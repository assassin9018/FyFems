namespace MyFems.Dto;

public class AttachmentDto : BaseDto
{
    public byte[] Content { get; set; }
}

public class ImageDto : AttachmentDto
{
}
