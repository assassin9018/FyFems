namespace MyFemsApi.Services.Impl;

internal class AttachmentsService : BaseService, IAttachmentsService
{
    public async Task<AttachmentDto> GetAttachment(int attachId)
    {
        return new AttachmentDto();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <returns>Идентификатор созданного файла.</returns>
    public async Task<int> PostAttachment(AttachmentDto image)
    {
        return int.MaxValue;
    }
}
