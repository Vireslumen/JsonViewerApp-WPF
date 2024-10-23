using JsonViewerApp.ViewModels;

namespace JsonViewerApp.Interfaces;

/// <summary>
///     Интерфейс для управления навигацией в приложении.
/// </summary>
public interface INavigationService
{
    /// <summary>
    ///     Открывает окно поиска с указанной моделью представления <see cref="SearchViewModel" />.
    /// </summary>
    /// <param name="viewModel">Модель представления для окна поиска.</param>
    void OpenSearchWindow(SearchViewModel viewModel);
}