using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyFemsApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ContactsController : Controller
{
    [HttpGet]
    public async Task<ActionResult<List<ContactDto>>> GetContacts()
    {
        return Ok(new List<ContactDto>());
    }

    [HttpGet("requests")]
    public async Task<ActionResult<List<ContactRequestDto>>> GetRequests()
    {
        return Ok(new List<ContactRequestDto>());
    }

    [HttpPost("requests/send/{contactId}")]
    public async Task<ActionResult<ContactRequestDto>> SendRequest(int contactId)
    {
        return Ok(new ContactRequestDto());
    }

    [HttpPatch("requests/apply/{requestId}")]
    public async Task<ActionResult<ContactRequestDto>> ApplyRequest(int requestId)
    {
        return Ok(new ContactRequestDto());
    }

    [HttpPatch("requests/decline/{requestId}")]
    public async Task<ActionResult<ContactRequestDto>> DeclineRequest(int requestId)
    {
        return Ok(new ContactRequestDto());
    }
}
