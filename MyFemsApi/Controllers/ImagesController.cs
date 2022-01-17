using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyFemsApi.Controllers;


[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ImagesController : Controller
{
    [HttpGet("{imageId}")]
    public async Task<ActionResult<ImageDto>> GetImage([Range(1, int.MaxValue)] int imageId)
    {
        return Ok(new ImageDto());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <returns>Идентификатор созданного изображения</returns>
    [HttpPost]
    public async Task<ActionResult<int>> PostImage([Required, FromBody] ImageDto image)
    {
        return Ok(int.MaxValue);
    }
}
