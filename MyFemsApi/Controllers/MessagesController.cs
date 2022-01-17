using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyFemsApi.Controllers;


[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MessagesController : Controller
{
    [HttpPost("{dialogId}")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDto request)
    {
        return Ok();
    }
}