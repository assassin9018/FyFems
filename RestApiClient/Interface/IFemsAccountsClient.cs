namespace RestApiClient;

public interface IFemsAccountsClient : IFemsBaseClient
{
    Task<bool> ChangePass(ChangePassRequest request);
    Task<string> LogIn(AuthRequest request);
    Task LogOut(int tokenId);
    Task<UserDto> Reg(RegUserRequest user);
}
