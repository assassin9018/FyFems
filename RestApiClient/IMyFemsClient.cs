
namespace RestApiClient;

public interface IMyFemsClient
{
    Task<ContactRequestDto> ApplyContactRequest(int requestId);
    Task<bool> ChangePass(ChangePassRequest request);
    Task<ContactRequestDto> DeclineContactRequest(int requestId);
    Task<AttachmentDto> GetAttachment(int attachId);
    Task<List<ContactRequestDto>> GetContactRequests();
    Task<List<ContactDto>> GetContacts();
    Task<List<DialogLastModifiedOnly>> GetDialogModificationDates();
    Task<List<DialogDto>> GetDialogs();
    Task<DialogUsersOnly> GetDialogUsers(int dialogId);
    Task<ImageDto> GetImage(int iamageId);
    Task<List<MessageDto>> GetMessages(DateTime lastUpdate);
    Task<List<MessageDto>> GetMessagesFromDialog(int dialogId, int lastMessageId);
    Task<UserDto> GetUser(int userId);
    Task<bool> IsServiceActive();
    Task<string> Login(AuthRequest request);
    Task<int> PostAttachment(AttachmentDto attachment);
    Task<DialogDto> PostConversation(ConversationRequest requestDto);
    Task<DialogDto> PostDialog(int contactId);
    Task<int> PostImage(ImageDto image);
    Task<MessageDto> PostMessage(int dialogId, MessageRequest request);
    Task<UserDto> Reg(RegUserDto user);
    Task<List<UserDto>> Search(string partOfUserName);
    Task<ContactRequestDto> SendContactRequest(int toUserId);
    Task<UserDto> WhoAmI();
    void UpdateCrendentials(string email, string password, string refreshToken);
}