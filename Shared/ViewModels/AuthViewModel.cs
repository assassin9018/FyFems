using ClientLocalDAL.Repository;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using MyFems.Clients.Shared.Messages;
using RestApiClient;

namespace MyFems.Clients.Shared.ViewModels;

public partial class AuthViewModel : ObservableObject
{
    private readonly IMyFemsFullClient _client;
    private readonly UnitOfWork _unitOfWork;

    [ObservableProperty]
    private bool _isLoggedIn;


    public AuthViewModel(IMyFemsFullClient client, UnitOfWork unitOfWork)
    {
        _client = client;
        _unitOfWork = unitOfWork;
    }

    private IRelayCommand? _showRegViewCommand;
    public IRelayCommand ShowRegViewCommand => _showRegViewCommand ??= new RelayCommand(() =>
    {
        WeakReferenceMessenger.Default.Send<ShowRegistrationViewMessage>();
    });
}
