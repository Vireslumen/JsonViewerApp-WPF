using System.Collections.Generic;
using System.Collections.ObjectModel;
using JsonViewerApp.Models;

namespace JsonViewerApp.Interfaces;

/// <summary>
///     Интерфейс для поиска элементов JSON дерева.
/// </summary>
public interface ISearchService
{
    /// <summary>
    ///     Ищет элементы в JSON дереве, которые соответствуют заданному запросу.
    /// </summary>
    /// <param name="items">Коллекция JSON элементов.</param>
    /// <param name="query">Поисковый запрос.</param>
    /// <returns>Список найденных элементов.</returns>
    List<JsonTreeItem> FindMatches(ObservableCollection<JsonTreeItem> items, string? query);
}