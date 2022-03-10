using ClientLocalDAL.Context;
using ClientViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyFemsWpfClient.Dialogs;
using MyFemsWpfClient.View;
using RestApiClient;
using System;
using System.Configuration;
using System.Windows;

namespace MyFemsWpfClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        ServiceCollection services = new();
        try
        {
            ConfigureServices(services);
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
        _serviceProvider = services.BuildServiceProvider();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        string dbConnection = ConfigurationManager.AppSettings.Get("DbConnection") ?? throw new NullReferenceException("Db connection string not found.");
        string serviceConnection = ConfigurationManager.AppSettings.Get("DbConnection") ?? throw new NullReferenceException("Messenger service connection string not found.");
        services.AddDbContext<SqLiteDbContext>(options =>
        {
            options.UseSqlite(dbConnection);
        });
        services.AddSingleton<ApplicationViewModel>();
        services.AddMyFemsClient(serviceConnection);
        services.AddSingleton<MainWindow>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IFileService, FileService>();
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetService<MainWindow>() ?? throw new NullReferenceException("Exception on dependency injection!");
        mainWindow.Show();
    }
}
