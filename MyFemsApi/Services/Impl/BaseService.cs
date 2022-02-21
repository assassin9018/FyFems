namespace MyFemsApi.Services.Impl;

internal class BaseService
{
    private protected static async Task<User> GetUser(UnitOfWork unit, int userId)
        => await unit.UserRepository.FindAsync(userId) ?? throw new ApiException();
}