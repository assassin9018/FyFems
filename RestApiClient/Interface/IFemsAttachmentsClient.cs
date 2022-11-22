namespace RestApiClient;

public interface IFemsAttachmentsClient : IFemsBaseClient
{
    Task<AttachmentDto> GetAttachment(int attachId);
    Task<int> PostAttachment(AttachmentDto attachment);
}
