using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyFemsApi.Controllers;


[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AttachmentsController : Controller
{
    [HttpGet("{attachId}")]
    public async Task<ActionResult<AttachmentDto>> GetAttachment([Range(1, int.MaxValue)] int attachId)
    {
        return Ok(new AttachmentDto());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <returns>Идентификатор созданного файла.</returns>
    [HttpPost]
    public async Task<ActionResult<int>> PostAttachment([Required, FromBody] ImageDto image)
    {
        return Ok(int.MaxValue);
    }
}
