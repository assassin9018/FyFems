using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using RestApiClient;

namespace MyFems.Clients.Shared.ViewModels;

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

    [ICommand]
    private async Task PingService()
    {
        try
        {
            _isConnected = await _client.IsServiceActive();
        }
        catch
        {
            _isConnected = false;
        }
    }

    #endregion
}