namespace MyFems.Services;

public interface IFileService
{
    FileStream OpenFile(string path);
    Task SaveFile(string path, Stream content);
    Task<string> SaveFile(Stream content);
    void DeleteFile(string path);
}
