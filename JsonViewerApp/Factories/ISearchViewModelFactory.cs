using System.Collections.ObjectModel;
using JsonViewerApp.Models;
using JsonViewerApp.ViewModels;

namespace JsonViewerApp.Factories;

/// <summary>
///     Интерфейс фабрики для создания экземпляров <see cref="SearchViewModel" />.
/// </summary>
public interface ISearchViewModelFactory
{
    /// <summary>
    ///     Создает экземпляр <see cref="SearchViewModel" /> с переданной коллекцией JSON элементов.
    /// </summary>
    /// <param name="treeItems">Коллекция JSON элементов для передачи в <see cref="SearchViewModel" />.</param>
    /// <returns>Новый экземпляр <see cref="SearchViewModel" />.</returns>
    SearchViewModel Create(ObservableCollection<JsonTreeItem> treeItems);
}