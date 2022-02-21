using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFemsApi.Services;
using System.ComponentModel.DataAnnotations;

namespace MyFemsApi.Controllers;

[Controller]
[Authorize]
[Route("api/[controller]")]
public class AccountsController : BaseController
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
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
        var result = await _accountService.Registration(regRequest);

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
    [HttpGet("LogIn")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        var result = await _accountService.LogIn(request);

        return Ok(result);
    }

    [HttpGet("LogOut")]
    public async Task<IActionResult> LogOut()
    {
        await _accountService.LogOut(int.MaxValue);

        return Ok();
    }

    [HttpPatch("ChangePass")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassRequest request)
    {
        await _accountService.ChangePassword(request, RequestUserId);

        return Ok();
    }
}
