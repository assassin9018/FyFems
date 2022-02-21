namespace MyFemsApi.Services;

public interface IAttachmentsService
{
    Task<AttachmentDto> GetAttachment(int attachId);
    Task<int> PostAttachment(AttachmentDto image);
}