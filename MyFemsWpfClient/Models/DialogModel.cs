using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace MyFemsWpfClient.Models;

internal partial class DialogModel : ObservableObject
{
    [ObservableProperty]
    private int _id;
    [ObservableProperty]
    private DateTime _lastUpdate;

    public ObservableCollection<UserModel> Users { get; set; } = new();
    public ObservableCollection<MessageModel> Messages { get; set; } = new();
}