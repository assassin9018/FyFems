using MyFemsApi.Services.Impl;

namespace MyFemsApi.Services;

public static class ServicesDI
{
    public static IServiceCollection AddFemsServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IAttachmentsService, AttachmentsService>()
            .AddScoped<IContactsService, ContactsService>()
            .AddScoped<IDialogsService, DialogsService>()
            .AddScoped<IImagesService, ImagesService>()
            .AddScoped<IMessagesService, MessagesService>()
            .AddScoped<IUsersService, UsersService>();
    }
}
