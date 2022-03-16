using Microsoft.Extensions.DependencyInjection;
using MyFems.ViewModels;

namespace MyFemsWpfClient;

public class ViewModelLocator
{
    public ApplicationViewModel ApplicationViewModel
        => App.ServiceProvider.GetRequiredService<ApplicationViewModel>();

    public AuthViewModel AuthViewModel
        => App.ServiceProvider.GetRequiredService<AuthViewModel>();

    public MainViewModel MainViewModel
        => App.ServiceProvider.GetRequiredService<MainViewModel>();
}
