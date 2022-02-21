using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ClientModels;

internal partial class CurrentUserModel : UserModel
{
    [ObservableProperty]
    private string _refreshToken = string.Empty;
    public ObservableCollection<ContactModel> Contacts { get; set; } = new();
    public ObservableCollection<DialogModel> Dialogs { get; set; } = new();
}