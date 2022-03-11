using MyFems.ViewModels;
using System.Windows;

namespace MyFemsWpfClient.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(ApplicationViewModel applicationVM)
    {
        DataContext = applicationVM;
        InitializeComponent();
    }
}
