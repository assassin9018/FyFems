namespace MyFemsApi.Services;

public interface IUsersService
{
    Task ChangePassword(ChangePassRequest request, int requestUserId);
    Task<UserDto> GetUser(int userId);
    Task<string> Login(AuthRequest request);
    Task<UserDto> Registration(RegUserDto regRequest);
    Task<List<UserDto>> Search(string searchText);
    Task<UserDto> WhoAmI(int requestUserId);
}