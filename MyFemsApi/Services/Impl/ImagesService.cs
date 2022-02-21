namespace MyFemsApi.Services.Impl;

internal class ImagesService : BaseService, IImagesService
{
    public async Task<ImageDto> GetImage(int imageId)
    {
        return new ImageDto();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <returns>Идентификатор созданного изображения</returns>
    public async Task<int> PostImage(ImageDto image)
    {
        return int.MaxValue;
    }
}
