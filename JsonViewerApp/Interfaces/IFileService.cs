namespace JsonViewerApp.Interfaces
{
    /// <summary>
    ///     Интерфейс для работы с файловыми операциями.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        ///     Получает размер файла в мегабайтах.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <returns>Размер файла в мегабайтах.</returns>
        double GetFileSizeInMegaBytes(string filePath);

        /// <summary>
        ///     Проверяет, что размер файла не превышает допустимое значение.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="maxSizeInMegaBytes">Максимальный размер в мегабайтах.</param>
        /// <returns>True, если файл не превышает допустимый размер, иначе false.</returns>
        bool ValidateFileSize(string filePath, double maxSizeInMegaBytes);
    }
}