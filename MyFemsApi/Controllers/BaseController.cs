using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyFemsApi.Controllers;

public class BaseController : Controller
{
    private protected int RequestUserId => int.Parse(HttpContext.User.FindFirstValue(Constants.UserIdClaimName) ?? "-1");
}
