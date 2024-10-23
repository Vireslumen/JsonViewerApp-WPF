using System.Collections.Generic;
using System.Threading.Tasks;
using JsonViewerApp.Models;

namespace JsonViewerApp.Interfaces
{
    /// <summary>
    ///     Интерфейс для асинхронного парсинга JSON файлов.
    /// </summary>
    public interface IJsonParser
    {
        /// <summary>
        ///     Асинхронно парсит JSON файл в дерево объектов.
        /// </summary>
        /// <param name="jsonFilePath">Путь к JSON файлу.</param>
        /// <returns>Список корневых элементов типа <see cref="JsonTreeItem" />.</returns>
        Task<List<JsonTreeItem>> ParseJsonAsync(string jsonFilePath);
    }
}