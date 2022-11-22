using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MyFems.Clients.Shared.Models;

public partial class DialogModel : ObservableObject
{
    [ObservableProperty]
    private int _id;
    [ObservableProperty]
    private string _name = string.Empty;
    [ObservableProperty]
    private DateTime _lastModified;
    [ObservableProperty]
    private bool _isPrivate;

    public ObservableCollection<UserModel> Users { get; set; } = new();
    public ObservableCollection<MessageModel> Messages { get; set; } = new();
}