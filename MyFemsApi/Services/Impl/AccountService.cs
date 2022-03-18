using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyFemsApi.Services.Impl;

internal class AccountService : BaseService, IAccountService
{
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;
    private readonly IServiceProvider _provider;

    public AccountService(IMapper mapper, UnitOfWork unitOfWork, IServiceProvider provider)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _provider = provider;
    }

    /// <summary>
    /// Метод регистрации нового пользователя.
    /// </summary>
    /// <param name="regRequest">Новый пользователь.</param>
    /// <returns>При успешной регистрации DTO нового пользователя.</returns>
    public async Task<UserDto> Registration(RegUserRequest regRequest)
    {
        PasswordHasher<User> hasher = _provider.GetService<PasswordHasher<User>>() ?? throw new InvalidOperationException();
        var dbUser = _mapper.Map<RegUserRequest, User>(regRequest);
        dbUser.PasswordHash = hasher.HashPassword(dbUser, regRequest.Password);
        dbUser.Created = DateTime.UtcNow;
        await _unitOfWork.UserRepository.SaveAsync(dbUser);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<User, UserDto>(dbUser);
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
    public async Task<string> LogIn(AuthRequest request)
    {
        PasswordHasher<User> hasher = _provider.GetService<PasswordHasher<User>>() ?? throw new InvalidOperationException();
        IConfiguration configuration = _provider.GetService<IConfiguration>() ?? throw new InvalidOperationException();

        User? user = (await _unitOfWork.UserRepository.GetAsync(x => x.Email == request.Email)).FirstOrDefault();
        if(user is null || hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
            throw new NotFoundException();

        ConfigOptions.JwtAuthOptions jwtOptions = new(configuration);
        var claims = new List<Claim> {
            new(Constants.UserIdClaimName, user.Id.ToString()),
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

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public async Task LogOut(int tokenId)
    { 
    }

    public async Task ChangePassword(ChangePassRequest request, int requestUserId)
    {
        PasswordHasher<User> hasher = _provider.GetService<PasswordHasher<User>>() ?? throw new InvalidOperationException();
        User user = await GetUser(_unitOfWork, requestUserId);

        if(hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
            throw new UnauthorizedException();

        user.PasswordHash = hasher.HashPassword(user, request.NewPassword);
        await _unitOfWork.UserRepository.SaveAsync(user);
        await _unitOfWork.SaveAsync();
    }
}
