namespace ClientViewModels;

public interface IDialogService
{
    void ShowMessage(string message);
    void ShowError(string message);
    string FilePath { get; set; }
    bool OpenFileDialog();
    bool SaveFileDialog();
}