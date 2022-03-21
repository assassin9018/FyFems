using ClientLocalDAL.Context;
using ClientLocalDAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFems.Clients.Shared.Models;
using MyFems.Clients.Shared.ViewModels;
using MyFems.Services;
using MyFemsWpfClient.Dialogs;
using MyFemsWpfClient.View;
using MyFemsWpfClient.Windows;
using RestApiClient;
using System;
using System.Reflection;
using System.Windows;

namespace MyFemsWpfClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;
    public static IServiceProvider ServiceProvider { get; private set; }

    public App()
    {
        try
        {
            _host = CreateHostBuilder().Build();
            ServiceProvider = _host.Services;
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[]? args = null)
    {
        return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("appsettings.json", optional: true);
                })
                .ConfigureServices(ConfigureServices);
    }

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        IConfigurationSection config = context.Configuration.GetSection(nameof(AppSettings));
        services.Configure<AppSettings>(config);

        string dbConnection = config.GetValue<string>(nameof(AppSettings.DbConnection));
        services.AddDbContext<SqLiteDbContext>(options =>
        {
            options.UseSqlite(dbConnection);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }, contextLifetime: ServiceLifetime.Singleton);

        string serviceUri = config.GetValue<string>(nameof(AppSettings.ServiceUri));
        services.AddMyFemsClient(serviceUri);

        services.AddAutoMapper(Assembly.GetAssembly(typeof(MyFems.Clients.Shared.MapperProfile)))
            .AddSingleton<ApplicationViewModel>()
            .AddSingleton<AuthViewModel>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<NewMessageViewModel>()
            .AddSingleton<MainWindow>()
            .AddSingleton<UnitOfWork>()
            .AddSingleton<IDialogService, DialogService>()
            .AddSingleton<IFileService, FileService>();

        services.AddSingleton<RegistrationViewModel>()
            .AddSingleton<RegistrationWindow>();
    }

    protected void OnStartup(object sender, StartupEventArgs e)
    {
        _host.Start();

        var mainWindow = ServiceProvider.GetService<MainWindow>();// ?? throw new NullReferenceException("Exception on dependency injection!");
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using(_host)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(1));
        }

        base.OnExit(e);
    }
}
