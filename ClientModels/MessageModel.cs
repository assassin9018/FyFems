using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ClientModels;

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

    public ObservableCollection<ImageModel> Images { get; set; } = new();
    public ObservableCollection<AttachmentModel> Attachments { get; set; } = new();

    public MessageModel(UserModel from)
    {
        _from = from;
    }
}