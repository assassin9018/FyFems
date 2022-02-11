using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace MyFemsWpfClient.ViewModels;

internal class ApplicationViewModel : ObservableObject
{
    private UserViewModel _userViewModel;
    [ObservableProperty]
    private bool _isLoggedIn;
    [ObservableProperty]
    private bool _isConnected;

    public ApplicationViewModel(UserViewModel userViewModel)
    {
        _userViewModel = userViewModel;
    }
}
