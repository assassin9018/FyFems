namespace MyFemsApi.Services;

public interface IImagesService
{
    Task<ImageDto> GetImage(int imageId);
    Task<int> PostImage(ImageDto image);
}
