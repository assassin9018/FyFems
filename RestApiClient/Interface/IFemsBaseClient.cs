namespace RestApiClient;

public interface IFemsBaseClient
{
    void UpdateCrendentials(string email, string password, string refreshToken);
    Task<bool> IsServiceActive();
}
