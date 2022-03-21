using Microsoft.Extensions.DependencyInjection;

namespace RestApiClient;

public static class DIExtention
{
    public static IServiceCollection AddMyFemsClient(this IServiceCollection services, string restServiceConnection)
    {
        services.AddSingleton<IMyFemsFullClient, MyFemsClient>(x => new MyFemsClient(restServiceConnection));

        services.AddSingleton<IFemsAccountsClient>(x => x.GetService<IMyFemsFullClient>()!);
        services.AddSingleton<IFemsAttachmentsClient>(x => x.GetService<IMyFemsFullClient>()!);
        services.AddSingleton<IFemsBaseClient>(x => x.GetService<IMyFemsFullClient>()!);
        services.AddSingleton<IFemsContactsClient>(x => x.GetService<IMyFemsFullClient>()!);
        services.AddSingleton<IFemsDialogsClient>(x => x.GetService<IMyFemsFullClient>()!);
        services.AddSingleton<IFemsImagesClient>(x => x.GetService<IMyFemsFullClient>()!);
        services.AddSingleton<IFemsMessagesClient>(x => x.GetService<IMyFemsFullClient>()!);
        services.AddSingleton<IFemsUsersClient>(x => x.GetService<IMyFemsFullClient>()!);

        return services;
    }
}