using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using RestApiClient;

namespace MyFems.ViewModels;

public partial class ApplicationViewModel : ObservableObject
{
    private readonly IMyFemsFullClient _client;

    [ObservableProperty]
    private AuthViewModel _authVM;
    [ObservableProperty]
    private MainViewModel _mainVM;
    [ObservableProperty]
    private bool _isConnected;

    public ApplicationViewModel(IMyFemsFullClient client, AuthViewModel authVM, MainViewModel mainVM)
    {
        _client = client;
        _authVM = authVM;
        _mainVM = mainVM;
    }

    #region Comands

    private IAsyncRelayCommand? _pingServiceCommand;
    public IAsyncRelayCommand PingService
        => _pingServiceCommand ??= new AsyncRelayCommand(async () =>
        {
            try
            {
                _isConnected = await _client.IsServiceActive();
            }
            catch
            {
                _isConnected = false;
            }
        });

    #endregion
}