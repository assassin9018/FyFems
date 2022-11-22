namespace RestApiClient;

public interface IFemsMessagesClient : IFemsBaseClient
{
    Task<List<MessageDto>> GetMessages(DateTime lastUpdate);
    Task<List<MessageDto>> GetMessagesFromDialog(int dialogId, int lastMessageId);
    Task<MessageDto> PostMessage(int dialogId, MessageRequest request);
}
