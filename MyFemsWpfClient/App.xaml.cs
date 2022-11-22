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
using MyFemsWpfClient.Helpers;
using MyFemsWpfClient.View;
using MyFemsWpfClient.Windows;
using RestApiClient;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace MyFemsWpfClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IAppLogger _logger;
    private readonly IHost _host;
    public static IServiceProvider ServiceProvider { get; private set; } = default!;

    public App()
    {
        try
        {
            _host = CreateHostBuilder().Build();
            ServiceProvider = _host.Services;
            _logger = ServiceProvider.GetService<IAppLogger>() ?? throw new ApplicationException("ILogger does not exist at service container.");
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            throw;
        }
    }

    public static IHostBuilder CreateHostBuilder()
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

        string dbConnection = config.GetValue<string>(nameof(AppSettings.DbConnection))!;
        services.AddDbContext<SqLiteDbContext>(options =>
        {
            options.UseSqlite(dbConnection);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }, contextLifetime: ServiceLifetime.Singleton);

        string serviceUri = config.GetValue<string>(nameof(AppSettings.ServiceUri))!;
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
            .AddSingleton<RegistrationWindow>(x => new() { Owner = x.GetRequiredService<MainWindow>() });

        services.AddSingleton<IAppLogger, AppLogger>();
    }

    protected void OnStartup(object sender, StartupEventArgs e)
    {
        _host.Start();
        this.DispatcherUnhandledException += UnhandledExceptionHandler;
        var mainWindow = ServiceProvider.GetService<MainWindow>() ?? throw new NullReferenceException("MainWindow does not exist at service container.");
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

    private void UnhandledExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        if(Debugger.IsAttached)
            Debugger.Break();

        _logger.Error(e.Exception);
    }
}
