using ClientLocalDAL.Repository;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using RestApiClient;

namespace MyFems.ViewModels;

public partial class ApplicationViewModel : ObservableObject
{
    private readonly IMyFemsFullClient _client;
    private readonly UnitOfWork _unitOfWork;

    [ObservableProperty]
    private bool _isLoggedIn;
    [ObservableProperty]
    private bool _isConnected;

    public ApplicationViewModel(IMyFemsFullClient client, UnitOfWork unitOfWork)
    {
        _client = client;
        _unitOfWork = unitOfWork;
    }

    #region Comands



    #endregion
}