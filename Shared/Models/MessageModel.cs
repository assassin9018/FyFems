using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MyFems.Clients.Shared.Models;

public partial class MessageModel : ObservableObject
{
    [ObservableProperty]
    private int _id;
    [ObservableProperty]
    private UserModel _from;
    [ObservableProperty]
    private string _text = string.Empty;
    [ObservableProperty]
    private DateTime _recived;
    [ObservableProperty]
    private bool _selfMessage;

    public ObservableCollection<ImageModel> Images { get; set; } = new();
    public ObservableCollection<AttachmentModel> Attachments { get; set; } = new();

    public MessageModel(UserModel from)
    {
        _from = from;
    }
}