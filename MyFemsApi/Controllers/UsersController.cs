using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFemsApi.Services;
using System.ComponentModel.DataAnnotations;

namespace MyFemsApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    /// <summary>
    /// Метод регистрации нового пользователя.
    /// </summary>
    /// <param name="regRequest">Новый пользователь.</param>
    /// <returns>При успешной регистрации DTO нового пользователя.</returns>
    [HttpPost("Reg")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Registration([Required, FromBody] RegUserDto regRequest)
    {
        var result = await _usersService.Registration(regRequest);

        return Ok(result);
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
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        var result = await _usersService.Login(request);

        return Ok(result);
    }

    /// <summary>
    /// Возвращает информацию о залогиненом пользователе.
    /// </summary>
    /// <returns>Информация о залогиненом пользователе.</returns>
    [HttpGet("WhoAmI")]
    public async Task<ActionResult<UserDto>> WhoAmI()
    {
        var result = await _usersService.WhoAmI(RequestUserId);

        return Ok(result);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> GetUser(int userId)
    {
        var result = await _usersService.GetUser(userId);

        return Ok(result);
    }

    [HttpGet("Search/{searchText}")]
    public async Task<IActionResult> Search(string searchText)
    {
        var results = await _usersService.Search(searchText);

        return Ok(results);
    }

    [HttpPatch("ChangePass")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassRequest request)
    {
        await _usersService.ChangePassword(request, RequestUserId);

        return Ok();
    }
}