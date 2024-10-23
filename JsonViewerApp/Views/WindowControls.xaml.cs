using System.Windows;

namespace JsonViewerApp.Views;

/// <summary>
///     Логика взаимодействия для WindowControls.xaml
/// </summary>
public partial class WindowControls
{
    public WindowControls()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Метод для закрытия окна
    /// </summary>
    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this)!.Close();
    }

    /// <summary>
    ///     Метод для развертывания окна
    /// </summary>
    private void MaximizeWindow(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        if (window != null) window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    /// <summary>
    ///     Метод для сворачивания окна
    /// </summary>
    private void MinimizeWindow(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this)!.WindowState = WindowState.Minimized;
    }
}