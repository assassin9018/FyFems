using Microsoft.Extensions.DependencyInjection;
using MyFems.Clients.Shared.ViewModels;

namespace MyFemsWpfClient;

#pragma warning disable CA1822 // Mark members as static
public class ViewModelLocator
{
    public ApplicationViewModel ApplicationViewModel
        => App.ServiceProvider.GetRequiredService<ApplicationViewModel>();

    public AuthViewModel AuthViewModel
        => App.ServiceProvider.GetRequiredService<AuthViewModel>();

    public MainViewModel MainViewModel
        => App.ServiceProvider.GetRequiredService<MainViewModel>();

    public NewMessageViewModel NewMessageViewModel
        => App.ServiceProvider.GetRequiredService<NewMessageViewModel>();

    public RegistrationViewModel RegistrationViewModel
        => App.ServiceProvider.GetRequiredService<RegistrationViewModel>();
}
#pragma warning restore CA1822 // Mark members as static
