using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFemsApi.Services;

namespace MyFemsApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
internal class DialogsController : BaseController
{
    private readonly IDialogsService _service;

    public DialogsController(IDialogsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetDialogs()
    {
        var results = await _service.GetDialogs(RequestUserId);

        return Ok(results);
    }

    [HttpPost("{contactId}")]
    public async Task<IActionResult> Post(int contactId)
    {
        var result = await _service.Post(contactId, RequestUserId);

        return Ok(result);
    }

    [HttpPost("conversation")]
    public async Task<IActionResult> PostConversation(ConversationRequest request)
    {
        var result = await _service.PostConversation(request, RequestUserId);

        return Ok(result);
    }

    [HttpGet("users/{dialogId}")]
    public async Task<IActionResult> GetDialogUsers(int dialogId)
    {
        var results = await _service.GetDialogUsers(dialogId, RequestUserId);

        return Ok(results);
    }

    [HttpGet("modificationDates")]
    public async Task<IActionResult> GetLastModificationDates()
    {
        var result = await _service.GetLastModificationDates(RequestUserId);

        return Ok(result);
    }
}