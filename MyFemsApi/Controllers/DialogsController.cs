using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyFemsApi.Controllers;


[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DialogsController
{
}
