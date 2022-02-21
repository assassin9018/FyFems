namespace RestApiClient;

public interface IFemsDialogsClient : IFemsBaseClient
{ 
    Task<List<DialogLastModifiedOnly>> GetDialogModificationDates();
    Task<List<DialogDto>> GetDialogs();
    Task<DialogUsersOnly> GetDialogUsers(int dialogId);
    Task<DialogDto> PostConversation(ConversationRequest requestDto);
    Task<DialogDto> PostDialog(int contactId);
}
