namespace MyFemsApi.Services;

public interface IUsersService
{
    Task<UserDto> GetUser(int userId);
    Task<List<UserDto>> Search(string searchText);
    Task<UserDto> WhoAmI(int requestUserId);
}