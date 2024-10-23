using System.Windows.Input;

namespace JsonViewerApp.Views;

/// <summary>
///     Логика взаимодействия для SearchWindow.xaml
/// </summary>
public partial class SearchWindow
{
    public SearchWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Метод для перетаскивания окна
    /// </summary>
    private void DragWindow(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }
}