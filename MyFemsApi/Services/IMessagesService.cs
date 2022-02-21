namespace MyFemsApi.Services;

public interface IMessagesService
{
    Task SendMessage(MessageRequest messageRequest, int fromUserId);
}
