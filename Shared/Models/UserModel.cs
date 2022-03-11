using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace MyFems.Models;

public partial class UserModel : ObservableObject
{
    [ObservableProperty]
    private int _id;
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(FullName))]
    private string _name = string.Empty;
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(FullName))]
    private string _surname = string.Empty;
    [ObservableProperty]
    private string _email = string.Empty;

    public string FullName => $"{_surname} {_name}";
}