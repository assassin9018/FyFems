using Microsoft.Extensions.DependencyInjection;

namespace RestApiClient;

public static class DIExtention
{
    public static IServiceCollection AddMyFemsClient(this IServiceCollection services, string restServiceConnection)
    {
        services.AddSingleton<IMyFemsFullClient, MyFemsClient>(x => new MyFemsClient(restServiceConnection));

        services.AddSingleton(x => x.GetService<IFemsAccountsClient>()!);
        services.AddSingleton(x => x.GetService<IFemsAttachmentsClient>()!);
        services.AddSingleton(x => x.GetService<IFemsBaseClient>()!);
        services.AddSingleton(x => x.GetService<IFemsContactsClient>()!);
        services.AddSingleton(x => x.GetService<IFemsDialogsClient>()!);
        services.AddSingleton(x => x.GetService<IFemsImagesClient>()!);
        services.AddSingleton(x => x.GetService<IFemsMessagesClient>()!);
        services.AddSingleton(x => x.GetService<IFemsUsersClient>()!);

        return services;
    }
}