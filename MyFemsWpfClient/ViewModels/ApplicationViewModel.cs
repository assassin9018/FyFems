using ClientLocalDAL.Repository;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MyFemsWpfClient.Models;
using RestApiClient;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MyFemsWpfClient.ViewModels;

internal class ApplicationViewModel : ObservableObject
{
    private readonly IMyFemsClient _client;
    private readonly UnitOfWork _unitOfWork;
    [ObservableProperty]
    private readonly UserModel _userModel = new();
    [ObservableProperty]
    private bool _isLoggedIn;
    [ObservableProperty]
    private bool _isConnected;
    public ObservableCollection<UserModel> Users { get; set; } = new();

    #region Comands declaration

    private IAsyncRelayCommand? _updateContactsCommand;
    public IAsyncRelayCommand UpdateContactsCommand 
        => _updateContactsCommand ??=new AsyncRelayCommand(UpdateContacts);

    #endregion

    public ApplicationViewModel(IMyFemsClient client, UnitOfWork unitOfWork)
    {
        _client = client;
        _unitOfWork = unitOfWork;
    }

    #region Comands implementation

    private Task UpdateContacts()
    {
        throw new NotImplementedException();
    }

    #endregion
}