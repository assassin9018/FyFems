namespace MyFemsApi.Services;

public interface IDialogsService
{
    Task<List<DialogDto>> GetDialogs(int requestUserId);
    Task<DialogUsersOnly> GetDialogUsers(int dialogId, int requestUserId);
    Task<List<DialogLastModifiedOnly>> GetLastModificationDates(int requestUserId);
    Task<DialogDto> Post(int contactId, int curUserId);
    Task<DialogDto> PostConversation(ConversationRequest request, int requestUserId);
}
