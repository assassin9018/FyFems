using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFemsApi.Services;

namespace MyFemsApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
internal class ContactsController : BaseController
{
    private readonly IContactsService _service;

    public ContactsController(IContactsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetContacts()
    {
        var results = await _service.GetContacts(RequestUserId);

        return Ok(results);
    }

    [HttpGet("requests")]
    public async Task<IActionResult> GetRequests()
    {
        var results = await _service.GetRequests(RequestUserId);

        return Ok(results);
    }

    [HttpPost("requests/send/{toUserId}")]
    public async Task<IActionResult> SendRequest(int toUserId)
    {
        var result = await _service.SendRequest(toUserId, RequestUserId);

        return Ok(result);
    }

    [HttpPatch("requests/apply/{requestId}")]
    public async Task<IActionResult> ApplyRequest(int requestId)
    {
        var result = await _service.ApplyRequest(requestId, RequestUserId);

        return Ok(result);
    }

    [HttpPatch("requests/decline/{requestId}")]
    public async Task<IActionResult> DeclineRequest(int requestId)
    {
        var result = await _service.DeclineRequest(requestId, RequestUserId);

        return Ok(result);
    }
}
