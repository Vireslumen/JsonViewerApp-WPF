using System.Collections.ObjectModel;
using JsonViewerApp.Interfaces;
using JsonViewerApp.Models;
using JsonViewerApp.ViewModels;

namespace JsonViewerApp.Factories;

/// <summary>
///     Фабрика для создания экземпляров <see cref="SearchViewModel" />.
/// </summary>
/// <remarks>
///     Инициализирует новый экземпляр <see cref="SearchViewModelFactory" /> с зависимостью <see cref="ISearchService" />.
/// </remarks>
/// <param name="searchService">Сервис для выполнения поиска.</param>
public class SearchViewModelFactory(ISearchService searchService) : ISearchViewModelFactory
{
    /// <summary>
    ///     Создает экземпляр <see cref="SearchViewModel" /> с переданной коллекцией JSON элементов.
    /// </summary>
    /// <param name="treeItems">Коллекция JSON элементов для передачи в <see cref="SearchViewModel" />.</param>
    /// <returns>Новый экземпляр <see cref="SearchViewModel" />.</returns>
    public SearchViewModel Create(ObservableCollection<JsonTreeItem> treeItems)
    {
        return new SearchViewModel(treeItems, searchService);
    }
}