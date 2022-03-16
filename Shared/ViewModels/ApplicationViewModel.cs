using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using RestApiClient;

namespace MyFems.ViewModels;

public partial class ApplicationViewModel : ObservableObject
{
    private readonly IMyFemsFullClient _client;

    [ObservableProperty]
    private bool _isConnected;

    public ApplicationViewModel(IMyFemsFullClient client)
    {
        _client = client;
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