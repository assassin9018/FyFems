namespace RestApiClient;

public interface IFemsContactsClient : IFemsBaseClient
{
    Task<ContactRequestDto> ApplyContactRequest(int requestId);
    Task<ContactRequestDto> DeclineContactRequest(int requestId);
    Task<List<ContactRequestDto>> GetContactRequests();
    Task<List<ContactDto>> GetContacts();
    Task<ContactRequestDto> SendContactRequest(int toUserId);
}
