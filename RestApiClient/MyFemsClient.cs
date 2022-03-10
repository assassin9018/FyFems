using RestSharp;
using System.Net;

namespace RestApiClient;

internal class MyFemsClient : IMyFemsFullClient
{
    private readonly string _authHeaderName = "Authorization";
    private readonly string _basePartUrl = "api/";
    private readonly string _serviceUrl;
    private readonly RestClient _libClient;
    private string? _refreshToken;
    private string? _accessToken;
    private string? _email;
    private string? _password;

    public MyFemsClient(string serviceUrl)
    {
        _serviceUrl = serviceUrl;
        _libClient = new(new RestClientOptions()
        {
            BaseUrl = new Uri(_serviceUrl + _basePartUrl),
            Timeout = 60,
        });
    }

    public MyFemsClient(string serviceUrl, string? email, string? password, string? refreshToken = null) : this(serviceUrl)
    {
        _email = email;
        _password = password;
        _refreshToken = refreshToken;

        if(email is not null && password is not null && _refreshToken is null)
            UpdateRefreshToken().ConfigureAwait(false).GetAwaiter().GetResult();
        if(_refreshToken is not null)
            UpdateAccessToken().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    #region Private methods

    private async Task<RestResponse> Execute(RestRequest request)
    {
        if(_accessToken is null)
            throw new InvalidOperationException("Token can not be null");

        return await _libClient.ExecuteAsync(request);
    }

    private async Task<T?> Execute<T>(RestRequest request) where T : class
    {
        if(_accessToken is null)
            throw new InvalidOperationException("Token can not be null");

        RestResponse<T> responce = await _libClient.ExecuteAsync<T>(request);
        return responce.Data;
    }

    private async Task<RestResponse> ExecuteWithAuth(RestRequest request)
    {
        if(_accessToken is null)
            throw new InvalidOperationException("Token can not be null");

        request = request.AddHeader(_authHeaderName, _accessToken);
        return await Execute(request);
    }

    private async Task<T?> ExecuteWithAuth<T>(RestRequest request) where T : class
    {
        if(_accessToken is null)
            throw new InvalidOperationException("Token can not be null");

        request = request.AddHeader(_authHeaderName, _accessToken);

        return await Execute<T>(request);
    }

    private async Task UpdateRefreshToken()
    {
        if(string.IsNullOrWhiteSpace(_email) || string.IsNullOrWhiteSpace(_password))
            throw new InvalidOperationException("Invalid credentials");
        _refreshToken = await LogIn(new()
        {
            Email = _email,
            Password = _password,
        });
    }

    private async Task UpdateAccessToken()
    {
        _accessToken = await LogIn(new()
        {
            Email = _email,
            Password = _password,
        });
    }

    #endregion

    #region Account

    private const string Accounts = $"{nameof(Accounts)}/";

    public async Task<UserDto> Reg(RegUserDto user)
    {
        var restRequest = new RestRequest($"{Accounts}Reg/", Method.Post)
            .AddJsonBody(user);

        var response = await Execute<UserDto>(restRequest);

        return response ?? throw RequestException.NullResponce<UserDto>();
    }

    public async Task<string> LogIn(AuthRequest request)
    {
        if(request.Email != _email || request.Password != _password)
            (_email, _password) = (request.Email, request.Password);

        var restRequest = new RestRequest($"{Accounts}Login/")
            .AddJsonBody(request);

        var response = await Execute(restRequest);

        return _refreshToken = response.Content ?? throw RequestException.NullResponce<UserDto>();
    }

    public async Task LogOut(int tokenId)
    {
        throw new NotImplementedException();
        //todo implement
    }

    public async Task<bool> ChangePass(ChangePassRequest request)
    {
        var restRequest = new RestRequest($"{Accounts}ChangePass/", Method.Patch)
            .AddJsonBody(request);

        var response = await Execute(restRequest);

        string content = response.Content ?? throw RequestException.NullResponce<bool>();
        return bool.TryParse(content, out bool result) && result;
    }

    #endregion

    #region Attachments

    public async Task<AttachmentDto> GetAttachment(int attachId)
    {
        throw new NotImplementedException();
        //todo implement
    }

    public async Task<int> PostAttachment(AttachmentDto attachment)
    {
        throw new NotImplementedException();
        //todo implement
    }

    #endregion

    #region Contacts

    private const string Contacts = $"{nameof(Contacts)}/";
    private const string Requests = $"{Contacts}{nameof(Requests)}/";

    public async Task<List<ContactDto>> GetContacts()
    {
        var restRequest = new RestRequest($"{Contacts}");

        var response = await ExecuteWithAuth<List<ContactDto>>(restRequest);

        return response ?? throw RequestException.NullResponce<List<ContactDto>>();
    }
    public async Task<List<ContactRequestDto>> GetContactRequests()
    {
        var restRequest = new RestRequest($"{Requests}");

        var response = await ExecuteWithAuth<List<ContactRequestDto>>(restRequest);

        return response ?? throw RequestException.NullResponce<List<ContactRequestDto>>();
    }

    public async Task<ContactRequestDto> SendContactRequest(int toUserId)
    {
        var restRequest = new RestRequest($"{Requests}send/{toUserId}", Method.Post);

        var response = await ExecuteWithAuth<ContactRequestDto>(restRequest);

        return response ?? throw RequestException.NullResponce<ContactRequestDto>();
    }

    public async Task<ContactRequestDto> ApplyContactRequest(int requestId)
    {
        var restRequest = new RestRequest($"{Requests}apply/{requestId}", Method.Patch);

        var response = await ExecuteWithAuth<ContactRequestDto>(restRequest);

        return response ?? throw RequestException.NullResponce<ContactRequestDto>();
    }

    public async Task<ContactRequestDto> DeclineContactRequest(int requestId)
    {
        var restRequest = new RestRequest($"{Requests}decline/{requestId}", Method.Patch);

        var response = await ExecuteWithAuth<ContactRequestDto>(restRequest);

        return response ?? throw RequestException.NullResponce<ContactRequestDto>();
    }

    #endregion

    #region Dialogs

    private const string Dialogs = $"{nameof(Dialogs)}/";

    public async Task<List<DialogDto>> GetDialogs()
    {
        var restRequest = new RestRequest($"{Dialogs}");

        var response = await ExecuteWithAuth<List<DialogDto>>(restRequest);

        return response ?? throw RequestException.NullResponce<List<DialogDto>>();
    }

    public async Task<DialogDto> PostDialog(int contactId)
    {
        var restRequest = new RestRequest($"{Dialogs}{contactId}", Method.Post);

        var response = await ExecuteWithAuth<DialogDto>(restRequest);

        return response ?? throw RequestException.NullResponce<DialogDto>();
    }

    public async Task<DialogDto> PostConversation(ConversationRequest requestDto)
    {
        var restRequest = new RestRequest($"{Dialogs}", Method.Post).
            AddBody(requestDto);

        var response = await ExecuteWithAuth<DialogDto>(restRequest);

        return response ?? throw RequestException.NullResponce<DialogDto>();
    }

    public async Task<List<DialogLastModifiedOnly>> GetDialogModificationDates()
    {
        var restRequest = new RestRequest($"{Dialogs}");

        var response = await ExecuteWithAuth<List<DialogLastModifiedOnly>>(restRequest);

        return response ?? throw RequestException.NullResponce<List<DialogLastModifiedOnly>>();
    }

    public async Task<DialogUsersOnly> GetDialogUsers(int dialogId)
    {
        var restRequest = new RestRequest($"{Dialogs}{dialogId}");

        var response = await ExecuteWithAuth<DialogUsersOnly>(restRequest);

        return response ?? throw RequestException.NullResponce<DialogUsersOnly>();
    }

    #endregion

    #region Images

    public async Task<ImageDto> GetImage(int iamageId)
    {
        throw new NotImplementedException();
        //todo implement
    }

    public async Task<int> PostImage(ImageDto image)
    {
        throw new NotImplementedException();
        //todo implement
    }

    #endregion

    #region Messages

    private const string Messages = $"{nameof(Messages)}/";

    public async Task<List<MessageDto>> GetMessages(DateTime lastUpdate)
    {
        var restRequest = new RestRequest($"{Messages}{lastUpdate.ToUniversalTime()}", Method.Get);

        var response = await ExecuteWithAuth<List<MessageDto>>(restRequest);

        return response ?? throw RequestException.NullResponce<List<MessageDto>>();
    }

    public async Task<List<MessageDto>> GetMessagesFromDialog(int dialogId, int lastMessageId)
    {
        var restRequest = new RestRequest($"{Messages}fromDialog?dialogId={dialogId}&lastMesId={lastMessageId}", Method.Get);

        var response = await ExecuteWithAuth<List<MessageDto>>(restRequest);

        return response ?? throw RequestException.NullResponce<List<MessageDto>>();
    }

    public async Task<MessageDto> PostMessage(int dialogId, MessageRequest request)
    {
        var restRequest = new RestRequest($"{Messages}{dialogId}", Method.Post)
            .AddJsonBody(request);

        var response = await ExecuteWithAuth<MessageDto>(restRequest);

        return response ?? throw RequestException.NullResponce<MessageDto>();
    }

    #endregion

    #region Users

    private const string Users = $"{nameof(Users)}/";

    public async Task<UserDto> GetUser(int userId)
    {
        var restRequest = new RestRequest($"{Users}{userId}");

        var response = await ExecuteWithAuth<UserDto>(restRequest);

        return response ?? throw RequestException.NullResponce<UserDto>();
    }

    public async Task<UserDto> WhoAmI()
    {
        var restRequest = new RestRequest($"{Users}WhoAmI/");

        var response = await ExecuteWithAuth<UserDto>(restRequest);

        return response ?? throw RequestException.NullResponce<UserDto>();
    }

    public async Task<List<UserDto>> Search(string partOfUserName)
    {
        var restRequest = new RestRequest($"{Users}Search/{partOfUserName}");

        var response = await ExecuteWithAuth<List<UserDto>>(restRequest);

        return response ?? throw RequestException.NullResponce<List<UserDto>>();
    }

    #endregion

    public void UpdateCrendentials(string email, string password, string refreshToken)
    {
        _email = email;
        _password = password;
        _refreshToken = refreshToken;
    }

    public async Task<bool> IsServiceActive()
    {
        var restRequest = new RestRequest("Ok", Method.Get);

        var response = await Execute(restRequest);

        return response.StatusCode == HttpStatusCode.OK && response.Content is not null;
    }
}