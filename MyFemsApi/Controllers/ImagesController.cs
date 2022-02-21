using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFemsApi.Services;
using System.ComponentModel.DataAnnotations;

namespace MyFemsApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ImagesController : Controller
{
    private readonly IImagesService _service;

    public ImagesController(IImagesService service)
    {
        _service = service;
    }

    [HttpGet("{imageId}")]
    public async Task<IActionResult> GetImage([Range(1, int.MaxValue)] int imageId)
    {
        ImageDto imageDto = await _service.GetImage(imageId);
        return Ok(imageDto);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <returns>Идентификатор созданного изображения</returns>
    [HttpPost]
    public async Task<IActionResult> PostImage([Required, FromBody] ImageDto image)
    {
        var result = await _service.PostImage(image);

        return Ok(result);
    }
}
