using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DtoUser = MyFems.Dto.UserDto;
using ModelUser = DAL.Models.User;

namespace MyFemsApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;

    public UsersController(IMapper mapper, UnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("Reg")]
    [AllowAnonymous]
    public async Task<IActionResult> Registration([Required, FromBody] MyFems.Dto.RegUserDto user)
    {
        var dbUser = _mapper.Map<MyFems.Dto.RegUserDto, ModelUser>(user);
        await _unitOfWork.UserRepository.SaveAsync(dbUser);
        return Ok(_mapper.Map<ModelUser, DtoUser>(dbUser));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="hasher"></param>
    /// <param name="request"></param>
    /// <returns>JWT Token.</returns>
    /// <remarks>При дальнейших запросах к API необходимо в Header'ах указывать  "Authorization": "Bearer " + token</remarks>
    /// <exception cref="NotFoundException"></exception>
    [HttpGet("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromServices] IConfiguration configuration, [FromServices] PasswordHasher<ModelUser> hasher, [FromBody] MyFems.Dto.AuthRequest request)
    {
        ModelUser? user = (await _unitOfWork.UserRepository.GetAsync(x => x.Email == request.Email)).FirstOrDefault();
        if(user is null || hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
            throw new NotFoundException();

        ConfigOptions.JwtAuthOptions jwtOptions = new(configuration);
        var claims = new List<Claim> {
            new(ClaimTypes.SerialNumber, user.Id.ToString()),
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

    [HttpGet("Search/{searchText}")]
    public async Task<IActionResult> Search(string searchText)
    {
        var users = (await _unitOfWork.UserRepository.GetAsync(u => u.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase))).ToList();

        List<DtoUser> dtoUsers = new(users.Count);
        foreach(var user in users)
            dtoUsers.Add(_mapper.Map<ModelUser, DtoUser>(user));

        return Ok(dtoUsers);
    }

    [HttpPatch("ChangePass")]
    public async Task<IActionResult> ChangePassword([FromBody] MyFems.Dto.ChangePassRequest request, [FromServices] PasswordHasher<ModelUser> hasher)
    {
        var user = (await _unitOfWork.UserRepository.GetAsync(u => u.Email == request.Email)).FirstOrDefault();
        if(user is null)
            throw new NotFoundException(nameof(user));

        if(hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
            Unauthorized();

        user.PasswordHash = hasher.HashPassword(user, request.NewPassword);
        await _unitOfWork.UserRepository.SaveAsync(user);
        await _unitOfWork.SaveAsync();

        return Ok();
    }
}
