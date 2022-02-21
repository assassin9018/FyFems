using MyFemsApi.Services.Impl;

namespace MyFemsApi.Services;

public static class ServicesDI
{
    public static IServiceCollection AddPublicFemsServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAttachmentsService, AttachmentsService>();
        serviceCollection.AddScoped<IContactsService, ContactsService>();
        serviceCollection.AddScoped<IDialogsService, DialogsService>();
        serviceCollection.AddScoped<IImagesService, ImagesService>();
        serviceCollection.AddScoped<IMessagesService, MessagesService>();
        serviceCollection.AddScoped<IUsersService, UsersService>();
        return serviceCollection;
    }

    internal static IServiceCollection AddPrivateFemsServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }
}
