namespace JsonViewerApp.Interfaces;

/// <summary>
///     Интерфейс для открытия диалоговых окон выбора файлов.
///     Предоставляет метод для открытия файла с фильтрацией по типам файлов.
/// </summary>
public interface IFileDialogService
{
    /// <summary>
    ///     Открывает диалоговое окно для выбора файла и возвращает путь к выбранному файлу.
    /// </summary>
    /// <param name="filter">Фильтр для типов файлов (например, "JSON Files (*.json)|*.json").</param>
    /// <returns>Путь к выбранному файлу или <c>null</c>, если выбор отменён.</returns>
    string OpenFile(string filter);
}