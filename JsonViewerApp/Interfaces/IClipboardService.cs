namespace JsonViewerApp.Interfaces;

/// <summary>
///     Интерфейс для работы с буфером обмена.
///     Предоставляет метод для копирования текста в буфер обмена.
/// </summary>
public interface IClipboardService
{
    /// <summary>
    ///     Копирует указанный текст в буфер обмена.
    /// </summary>
    /// <param name="text">Текст для копирования в буфер обмена.</param>
    void SetText(string text);
}