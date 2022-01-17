using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MyFemsApi.Controllers;

/// <summary>
/// Контроллер проверки доступности сервиса.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OkController : Controller
{
    /// <summary>
    /// Сервис доступен.
    /// </summary>
    /// <returns>I am OK! Code 200</returns>
    /// <response code="200">I am OK!</response>
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public OkObjectResult Get()
        => Ok("I am OK!");
}