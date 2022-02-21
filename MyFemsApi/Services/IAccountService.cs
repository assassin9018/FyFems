namespace MyFemsApi.Services;

public interface IAccountService
{
    Task<UserDto> Registration(RegUserDto regRequest);
    Task ChangePassword(ChangePassRequest request, int requestUserId);
    Task<string> LogIn(AuthRequest request);
    Task LogOut(int tokenId);
}