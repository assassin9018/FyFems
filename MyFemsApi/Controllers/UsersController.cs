using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFemsApi.Services;

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

}