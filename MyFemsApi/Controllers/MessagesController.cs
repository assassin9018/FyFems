using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFemsApi.Services;

namespace MyFemsApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MessagesController : BaseController
{
    private readonly IMessagesService _service;

    public MessagesController(IMessagesService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] MessageRequest messageRequest)
    {
        await _service.SendMessage(messageRequest, RequestUserId);

        return Ok();
    }
}