using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JsonViewerApp.Interfaces;
using JsonViewerApp.Models;

namespace JsonViewerApp.Services;

/// <summary>
///     Сервис для поиска элементов в JSON дереве.
/// </summary>
public class SearchService : ISearchService
{
    /// <inheritdoc />
    public List<JsonTreeItem> FindMatches(ObservableCollection<JsonTreeItem> items, string? query)
    {
        var matches = new List<JsonTreeItem>();
        foreach (var item in items)
            FindMatchesRecursive(item, query, matches);
        return matches;
    }

    /// <summary>
    ///     Рекурсивно ищет элементы в дереве JSON, которые соответствуют поисковому запросу.
    /// </summary>
    /// <param name="item">Текущий элемент дерева для проверки.</param>
    /// <param name="query">Поисковый запрос, с которым сравниваются значения элементов.</param>
    /// <param name="matches">Список найденных элементов, соответствующих запросу.</param>
    private static void FindMatchesRecursive(JsonTreeItem item, string? query, List<JsonTreeItem> matches)
    {
        if (query != null && ((!string.IsNullOrEmpty(item.Name) && item.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                              (!string.IsNullOrEmpty(item.Value) && item.Value.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)))
            matches.Add(item);

        foreach (var child in item.Children)
            FindMatchesRecursive(child, query, matches);
    }
}