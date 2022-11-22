namespace RestApiClient;

public interface IMyFemsFullClient : IFemsBaseClient,
    IFemsAccountsClient,
    IFemsAttachmentsClient,
    IFemsContactsClient,
    IFemsDialogsClient,
    IFemsImagesClient,
    IFemsMessagesClient,
    IFemsUsersClient
{
}