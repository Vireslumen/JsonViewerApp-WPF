using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JsonViewerApp.Interfaces;
using JsonViewerApp.Models;

namespace JsonViewerApp.ViewModels;

/// <summary>
///     ViewModel для окна поиска по JSON-данным.
///     Содержит логику поиска, навигации по результатам и управление состоянием плейсхолдера.
/// </summary>
public class SearchViewModel(ObservableCollection<JsonTreeItem> treeItems, ISearchService searchService)
    : SearchViewModelBase
{
    /// <summary>
    ///     Коллекция результатов поиска.
    /// </summary>
    public ObservableCollection<JsonTreeItem> SearchResults { get; } = new();

    /// <summary>
    ///     Переходит к следующему результату поиска и обновляет отображение результатов.
    /// </summary>
    protected override void ExecuteNextSearchResult()
    {
        if (AllMatches is not {Count: > 0}) return;
        CurrentMatchIndex = (CurrentMatchIndex + 1) % AllMatches.Count;
        UpdateSearchResults();
        OnPropertyChanged(nameof(CurrentMatch));
    }

    /// <summary>
    ///     Переходит к предыдущему результату поиска и обновляет отображение результатов.
    /// </summary>
    protected override void ExecutePreviousSearchResult()
    {
        if (AllMatches is not {Count: > 0}) return;
        CurrentMatchIndex = (CurrentMatchIndex - 1 + AllMatches.Count) % AllMatches.Count;
        UpdateSearchResults();
        OnPropertyChanged(nameof(CurrentMatch));
    }

    /// <summary>
    ///     Выполняет поиск по дереву JSON и обновляет результаты.
    /// </summary>
    protected override void PerformSearch()
    {
        AllMatches = searchService.FindMatches(treeItems, SearchQuery);
        CurrentMatchIndex = 0;
        TotalMatches = AllMatches.Count;
        UpdateSearchResults();
        OnPropertyChanged(nameof(CurrentMatch));
    }

    /// <summary>
    ///     Строит дерево JSON из пути к выбранному элементу.
    /// </summary>
    /// <param name="path">Список элементов, представляющий путь.</param>
    /// <returns>Корневой элемент построенного дерева.</returns>
    private static JsonTreeItem? BuildTreeFromPath(List<JsonTreeItem> path)
    {
        JsonTreeItem? root = null;
        JsonTreeItem? currentParent = null;
        foreach (var item in path)
        {
            var clonedItem = item.CloneWithoutChildren();
            clonedItem.IsExpanded = true;

            if (currentParent != null)
            {
                clonedItem.Parent = currentParent;
                currentParent.Children.Add(clonedItem);
            }
            else
            {
                root = clonedItem;
            }

            currentParent = clonedItem;
        }

        return root;
    }

    /// <summary>
    ///     Получает путь к указанному элементу дерева.
    /// </summary>
    /// <param name="item">Целевой элемент.</param>
    /// <returns>Список элементов, представляющий путь к целевому элементу.</returns>
    private static List<JsonTreeItem> GetPathToItem(JsonTreeItem item)
    {
        var path = new List<JsonTreeItem>();
        var current = item;
        while (current != null)
        {
            path.Insert(0, current);
            current = current.Parent;
        }

        return path;
    }

    /// <summary>
    ///     Обновляет коллекцию результатов поиска для отображения в интерфейсе.
    /// </summary>
    private void UpdateSearchResults()
    {
        if (!AllMatches.Any()) return;

        var pathToItem = GetPathToItem(AllMatches[CurrentMatchIndex]);
        var rootItem = BuildTreeFromPath(pathToItem);

        if (rootItem == null) return;

        SearchResults.Clear();
        SearchResults.Add(rootItem);
    }
}