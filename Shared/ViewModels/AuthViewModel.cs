using ClientLocalDAL.Models;
using ClientLocalDAL.Repository;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using MyFems.Clients.Shared.Messages;
using MyFems.Clients.Shared.Models;
using MyFems.Clients.Shared.Services;
using MyFems.Dto;
using RestApiClient;
using System.Collections.ObjectModel;

namespace MyFems.Clients.Shared.ViewModels;

public partial class AuthViewModel : ObservableObject
{
    private readonly IMyFemsFullClient _client;
    private readonly UnitOfWork _unitOfWork;

    [ObservableProperty]
    private bool _isLoggedIn;
    [ObservableProperty]
    private string _email = string.Empty;
    [ObservableProperty]
    private string _exMessage = string.Empty;
    [ObservableProperty]
    private bool _savePassword;
    [ObservableProperty]
    private bool _stayAuthed;

    public ObservableCollection<StateModel> States = new();
    [ObservableProperty]
    public StateModel? _selectedState;


    public AuthViewModel(IMyFemsFullClient client, UnitOfWork unitOfWork)
    {
        _client = client;
        _unitOfWork = unitOfWork;
    }

    [ICommand]
    private void ShowRegView()
    {
        WeakReferenceMessenger.Default.Send<ShowRegistrationViewMessage>();
    }

    [ICommand]
    public async Task Login(IPasswordProvider passwordProvider)
    {
        AuthRequest request = new()
        {
            Email = Email,
            Password = passwordProvider.GetPassword(),
        };        
        try
        {
            string token = await _client.LogIn(request);
            _isLoggedIn = true;
            var user = await _client.WhoAmI();

            if(SelectedState is not null && Email == SelectedState.Email)
            {
                _unitOfWork.StateRepository.Update(new()
                {
                    Id = SelectedState.Id,
                    Email = SelectedState.Email,
                    Password = SavePassword ? request.Password : string.Empty,
                    RefrashToken = StayAuthed ? token : string.Empty,
                    SavePassword = SavePassword,
                    Theme = AppTheme.System,
                    //Avatar = user.Avatar.Id, todo add avatar for user
                });
            }
            else if(SavePassword)
            {
                _unitOfWork.StateRepository.Save(new()
                {
                    Id = user.Id,
                    Email = Email,
                    Password = request.Password,
                    RefrashToken = StayAuthed ? token : string.Empty,
                    SavePassword = SavePassword,
                    Theme = AppTheme.System,
                    //Avatar = user.Avatar.Id, todo add avatar for user
                });
            }

            _unitOfWork.Save();
        }
        catch(RequestException ex)
        { 
            _isLoggedIn = false;
            ExMessage = ex.Message;
        }
    }
}