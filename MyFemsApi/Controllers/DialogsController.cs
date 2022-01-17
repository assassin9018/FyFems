using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyFemsApi.Controllers;


[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DialogsController : Controller
{
    [HttpGet]
    public async Task<ActionResult<List<DialogDto>>> GetDialogs()
    {
        return new List<DialogDto>();
    }

    [HttpPost("{contactId}")]
    public async Task<ActionResult<DialogDto>> Post(int contactId)
    {
        return Ok(new DialogDto());
    }
}
