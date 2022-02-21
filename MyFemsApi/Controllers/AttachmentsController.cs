using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFemsApi.Services;
using System.ComponentModel.DataAnnotations;

namespace MyFemsApi.Controllers;


[ApiController]
[Authorize]
[Route("api/[controller]")]
internal class AttachmentsController : Controller
{
    private readonly IAttachmentsService _service;

    public AttachmentsController(IAttachmentsService service)
    {
        _service = service;
    }

    [HttpGet("{attachId}")]
    public async Task<ActionResult<AttachmentDto>> GetAttachment([Range(1, int.MaxValue)] int attachId)
    {
        AttachmentDto value = await _service.GetAttachment(attachId);
        return Ok(value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attachment"></param>
    /// <returns>Идентификатор созданного файла.</returns>
    [HttpPost]
    public async Task<ActionResult<int>> PostAttachment([Required, FromBody] AttachmentDto attachment)
    {
        int value = await _service.PostAttachment(attachment);
        return Ok(value);
    }
}
