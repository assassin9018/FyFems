using ClientViewModels;
using Microsoft.Win32;
using System;
using System.Windows;

namespace MyFemsWpfClient.Dialogs;

internal class DialogService : IDialogService
{
    public string FilePath { get; set; } = String.Empty;

    public bool OpenFileDialog()
    {
        OpenFileDialog openFileDialog = new();
        if(openFileDialog.ShowDialog() == true)
        {
            FilePath = openFileDialog.FileName;
            return true;
        }
        return false;
    }

    public bool SaveFileDialog()
    {
        SaveFileDialog saveFileDialog = new();
        if(saveFileDialog.ShowDialog() == true)
        {
            FilePath = saveFileDialog.FileName;
            return true;
        }
        return false;
    }

    public void ShowMessage(string message)
        => MessageBox.Show(message, "Message", MessageBoxButton.OK, MessageBoxImage.Information);

    public void ShowError(string message)
        => MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
}