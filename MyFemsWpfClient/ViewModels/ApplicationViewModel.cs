using ClientLocalDAL.Repository;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MyFems.Dto;
using MyFemsWpfClient.Models;
using RestApiClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyFemsWpfClient.ViewModels;

internal partial class ApplicationViewModel : ObservableObject
{
    private readonly IMyFemsFullClient _client;
    private readonly UnitOfWork _unitOfWork;
    [ObservableProperty]
    private bool _isLoggedIn;
    [ObservableProperty]
    private bool _isConnected;
    public ObservableCollection<UserModel> Users { get; set; } = new();
    public ObservableCollection<ContactModel> Contacts { get; set; } = new();
    public UserModel UserModel { get; } = new();

    #region Comands

    private IAsyncRelayCommand? _updateContactsCommand;
    public IAsyncRelayCommand UpdateContactsCommand 
        => _updateContactsCommand ??=new AsyncRelayCommand(async (CancalationToken) =>
        {
            List<ContactDto> contactsDto = await _client.GetContacts();
            var existed = Contacts.Select(x => x.Id).ToHashSet();

        });

    #endregion

    public ApplicationViewModel(IMyFemsFullClient client, UnitOfWork unitOfWork)
    {
        _client = client;
        _unitOfWork = unitOfWork;
    }
}