using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyFemsApi.Controllers;
public class BaseController : Controller
{
    private protected const string UserIdClaimName = "uid";
    private protected int RequestUserId => int.Parse(HttpContext.User.FindFirstValue("uid") ?? "-1");

    private protected async Task<User> GetRequestUser(IRepository<User> repository) 
        => await repository.FindAsync(RequestUserId) ?? throw new ApiException();
}
