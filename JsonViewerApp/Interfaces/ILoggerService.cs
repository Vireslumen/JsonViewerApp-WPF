using System;

namespace JsonViewerApp.Interfaces
{
    /// <summary>
    ///     Интерфейс для логирования событий приложения.
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        ///     Закрывает и очищает лог.
        /// </summary>
        void CloseAndFlush();

        /// <summary>
        ///     Записывает сообщение об ошибке в лог.
        /// </summary>
        /// <param name="exception">Исключение, которое необходимо зафиксировать.</param>
        /// <param name="message">Сообщение, связанное с ошибкой.</param>
        void Error(Exception exception, string message);

        /// <summary>
        ///     Записывает информационное сообщение в лог.
        /// </summary>
        /// <param name="message">Информационное сообщение.</param>
        void Information(string message);
    }
}