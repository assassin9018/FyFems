using Microsoft.Extensions.DependencyInjection;

namespace RestApiClient;

public static class DIExtention
{
    public static IServiceCollection AddMyFemsClient(this IServiceCollection services, string restServiceConnection)
    => services.AddSingleton<IMyFemsClient, MyFemsClient>(x => new MyFemsClient(restServiceConnection));
}