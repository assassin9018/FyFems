namespace MyFemsApi.Services;

public interface IContactsService
{
    Task<ContactRequestDto> ApplyRequest(int requestId, int requestUserId);
    Task<ContactRequestDto> DeclineRequest(int requestId, int requestUserId);
    Task<List<ContactDto>> GetContacts(int requestUserId);
    Task<List<ContactRequestDto>> GetRequests(int requestUserId);
    Task<ContactRequestDto> SendRequest(int toUserId, int fromUserId);
}
