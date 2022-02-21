namespace RestApiClient;

public interface IFemsImagesClient : IFemsBaseClient
{
    Task<ImageDto> GetImage(int iamageId);
    Task<int> PostImage(ImageDto image);
}
