using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyFemsApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;

    public UsersController(IMapper mapper, UnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Метод регистрации нового пользователя.
    /// </summary>
    /// <param name="regRequest">Новый пользователь.</param>
    /// <returns>При успешной регистрации DTO нового пользователя.</returns>
    [HttpPost("Reg")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Registration([Required, FromBody] RegUserDto regRequest, [FromServices] PasswordHasher<User> hasher)
    {
        var dbUser = _mapper.Map<RegUserDto, User>(regRequest);
        dbUser.PasswordHash = hasher.HashPassword(dbUser, regRequest.Password);
        await _unitOfWork.UserRepository.SaveAsync(dbUser);
        await _unitOfWork.SaveAsync();
        return Ok(_mapper.Map<User, UserDto>(dbUser));
    }

    /// <summary>
    /// Метод аутентификации пользователя.
    /// </summary>
    /// <param name="configuration">Конфигурация сервиса.</param>
    /// <param name="hasher">Класс для хэширования и валидации паролей.</param>
    /// <param name="request">Запрос авторизации.</param>
    /// <returns>JWT Token.</returns>
    /// <remarks>При дальнейших запросах к API необходимо в Header'ах указывать  "Authorization": "Bearer " + token</remarks>
    /// <exception cref="NotFoundException">При ошибке в паре почта\пароль.</exception>
    [HttpGet("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromServices] IConfiguration configuration, [FromServices] PasswordHasher<User> hasher, [FromBody] AuthRequest request)
    {
        User? user = (await _unitOfWork.UserRepository.GetAsync(x => x.Email == request.Email)).FirstOrDefault();
        if(user is null || hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
            throw new NotFoundException();

        ConfigOptions.JwtAuthOptions jwtOptions = new(configuration);
        var claims = new List<Claim> {
            new(UserIdClaimName, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.MobilePhone, user.Phone)
        };
        var jwt = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)), // время действия 2 минуты
                signingCredentials: new SigningCredentials(jwtOptions.GetSymmetricKey(), SecurityAlgorithms.HmacSha256));

        return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
    }

    /// <summary>
    /// Возвращает информацию о залогиненом пользователе.
    /// </summary>
    /// <returns>Информация о залогиненом пользователе.</returns>
    [HttpGet("WhoAmI")]
    public async Task<ActionResult<UserDto>> WhoAmI()
    {
        User currentUser = (await GetRequestUser(_unitOfWork.UserRepository));
        return Ok(_mapper.Map<User, UserDto>(currentUser));
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> GetUser(int userId)
    {
        User? user = await _unitOfWork.UserRepository.FindAsync(userId) ?? throw new ApiException();
        return Ok(_mapper.Map<User, UserDto>(user));
    }

    [HttpGet("Search/{searchText}")]
    public async Task<IActionResult> Search(string searchText)
    {
        var users = (await _unitOfWork.UserRepository.GetAsync(u => u.FullName.ToLower().Contains(searchText.ToLower()))).ToList();

        List<UserDto> dtoUsers = new(users.Count);
        foreach(var user in users)
            dtoUsers.Add(_mapper.Map<User, UserDto>(user));

        return Ok(dtoUsers);
    }

    [HttpPatch("ChangePass")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassRequest request, [FromServices] PasswordHasher<User> hasher)
    {
        User user = await GetRequestUser(_unitOfWork.UserRepository);

        if(hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
            Unauthorized();

        user.PasswordHash = hasher.HashPassword(user, request.NewPassword);
        await _unitOfWork.UserRepository.SaveAsync(user);
        await _unitOfWork.SaveAsync();

        return Ok();
    }
}