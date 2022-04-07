using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace MyFems.Clients.Shared.ViewModels;

public partial class NewMessageViewModel : ObservableObject
{
    [ObservableProperty]
    private string _message;

    [ICommand]
    private async Task SendMessage()
    {
        Message = string.Empty;
    }
}
