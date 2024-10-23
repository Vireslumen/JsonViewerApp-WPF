using JsonViewerApp.Interfaces;
using JsonViewerApp.ViewModels;
using JsonViewerApp.Views;

namespace JsonViewerApp.Services;

/// <summary>
///     Реализация <see cref="INavigationService" />, отвечающая за навигацию в приложении.
/// </summary>
public class NavigationService : INavigationService
{
    /// <summary>
    ///     Открывает окно поиска с указанной моделью представления <see cref="SearchViewModel" />.
    /// </summary>
    /// <param name="viewModel">Модель представления для окна поиска.</param>
    public void OpenSearchWindow(SearchViewModel viewModel)
    {
        var searchWindow = new SearchWindow
        {
            DataContext = viewModel
        };
        searchWindow.Show();
    }
}