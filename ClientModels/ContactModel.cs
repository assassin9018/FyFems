using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ClientModels;

public partial class ContactModel : ObservableObject
{
    [ObservableProperty]
    private int _id;
    [ObservableProperty]
    private DateTime _lastUpdate;
    [ObservableProperty]
    private bool _isOnline;
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(FullName))]
    private string _name = string.Empty;
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(FullName))]
    private string _surname = string.Empty;
    [ObservableProperty]
    private string _email = string.Empty;
    [ObservableProperty]
    private DialogModel? _dialog;

    public string FullName => $"{_surname} {_name}";
}