using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace MyFems.Clients.Shared.Models;

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
    [ObservableProperty]
    private DateOnly _birthDate;
    [ObservableProperty]
    private string _icon;

    public string FullName => $"{_surname} {_name}";
}