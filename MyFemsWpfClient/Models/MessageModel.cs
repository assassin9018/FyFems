using Microsoft.Toolkit.Mvvm.ComponentModel;
using MyFemsWpfClient.Models;
using System;
using System.Collections.ObjectModel;

namespace MyFemsWpfClient.Models;

internal partial class MessageModel : ObservableObject
{
    [ObservableProperty]
    private int _id;
    [ObservableProperty]
    private UserModel _from;
    [ObservableProperty]
    private string _text = string.Empty;
    [ObservableProperty]
    private DateTime _recived;

    public ObservableCollection<ImageInfo> Images { get; set; } = new();
    public ObservableCollection<AttachmentInfo> Attachments { get; set; } = new();

    public MessageModel(UserModel from)
    {
        _from = from;
    }
}