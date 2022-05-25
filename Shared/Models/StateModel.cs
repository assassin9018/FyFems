using ClientLocalDAL.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace MyFems.Clients.Shared.Models;

public class StateModel
{
    [ObservableProperty]
    private int _id;
    [ObservableProperty]
    private string _email = string.Empty;
    [ObservableProperty]
    private string _password  = string.Empty;
    [ObservableProperty]
    private bool _savePassword;
    [ObservableProperty]
    private string? _refrashToken;
    [ObservableProperty]
    private Guid _avatar;
    [ObservableProperty]
    private AppTheme _theme;
}
