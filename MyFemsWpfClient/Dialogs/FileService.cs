using ClientViewModels;
using System.IO;
using System.Threading.Tasks;

namespace MyFemsWpfClient.Dialogs;

internal class FileService : IFileService
{
    public FileStream OpenFile(string path)
        => new(path, FileMode.Open, FileAccess.Read);

    public async Task<string> SaveFile(Stream content)
    {
        string path = Path.GetTempFileName();
        await SaveFile(path, content);
        return path;
    }

    public async Task SaveFile(string path, Stream content)
    {
        if(new FileInfo(path).Length > 0)
            File.Delete(path);

        using FileStream fs = new(path, FileMode.OpenOrCreate, FileAccess.Write);
        await content.CopyToAsync(fs);
    }

    public void DeleteFile(string path)
    {
        if(File.Exists(path))
            File.Delete(path);
    }
}