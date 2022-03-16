using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace MyFems.Clients.Shared.ViewModels;

public partial class NewMessageViewModel : ObservableObject
{
    [ObservableProperty]
    private string _message;

    private IAsyncRelayCommand? _sendMessageCommand;
    public IAsyncRelayCommand SendMessage
        => _sendMessageCommand ??= new AsyncRelayCommand(()
            =>
        {
            Message = string.Empty;
            return Task.CompletedTask;
        });
}
