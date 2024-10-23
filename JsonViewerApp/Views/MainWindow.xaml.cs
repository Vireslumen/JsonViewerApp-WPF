using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using JsonViewerApp.ViewModels;

namespace JsonViewerApp.Views;

/// <summary>
///     Логика взаимодействия для окна MainWindow.
///     Окно отвечает за отображение данных JSON в виде дерева с динамическими отступами.
/// </summary>
public partial class MainWindow
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    /// <summary>
    ///     Метод для перетаскивания окна
    /// </summary>
    private void DragWindow(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left) DragMove();
    }

    /// <summary>
    ///     Метод перемещения на выбранный элемент
    /// </summary>
    private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
    {
        if (sender is not TreeViewItem item) return;
        item.BringIntoView();
        e.Handled = true;
    }
}