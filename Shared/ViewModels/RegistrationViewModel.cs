﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MyFems.Clients.Shared.Messages;
using MyFems.Dto;
using MyFems.Services;
using RestApiClient;

namespace MyFems.Clients.Shared.ViewModels;

public partial class RegistrationViewModel : ObservableObject
{
    private readonly IFemsAccountsClient _client;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private string _name = string.Empty;
    [ObservableProperty]
    private string _surname = string.Empty;
    [ObservableProperty]
    private string _email = string.Empty;
    [ObservableProperty]
    private string _phone = string.Empty;
    [ObservableProperty]
    private DateTime _birthDate = DateTime.Now.AddYears(-10);

    public RegistrationViewModel(IFemsAccountsClient client, IDialogService dialogService)
    {
        _client = client;
        _dialogService = dialogService;
    }

    [ICommand]
    private async Task SendRegRequest()
    {
        RegUserRequest request = new()
        {
            Surname = Surname,
            Name = Name,
            Email = Email,
            Phone = Phone,
            BirthDate = BirthDate,
            //todo Password = Password,
        };

        var result = await _client.Reg(request);

        if(result is not null)
        {
            _dialogService.ShowMessage("Регистрация прошла успешно!");
            WeakReferenceMessenger.Default.Send<CloseWindowMessage>();
        }
    }
}
