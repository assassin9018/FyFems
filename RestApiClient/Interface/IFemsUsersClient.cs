namespace RestApiClient;

public interface IFemsUsersClient : IFemsBaseClient
{
    Task<UserDto> GetUser(int userId);
    Task<List<UserDto>> Search(string partOfUserName);
    Task<UserDto> WhoAmI();
}
