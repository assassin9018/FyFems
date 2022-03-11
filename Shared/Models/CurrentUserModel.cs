using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MyFems.Models;

internal partial class CurrentUserModel : UserModel
{
    [ObservableProperty]
    private string _refreshToken = string.Empty;
    public ObservableCollection<ContactModel> Contacts { get; set; } = new();
    public ObservableCollection<DialogModel> Dialogs { get; set; } = new();
}