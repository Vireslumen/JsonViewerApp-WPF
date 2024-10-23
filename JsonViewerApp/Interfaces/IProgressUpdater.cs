using System;
using System.Threading.Tasks;

namespace JsonViewerApp.Interfaces
{
    /// <summary>
    ///     Интерфейс для обновления прогресса выполнения длительных задач.
    /// </summary>
    public interface IProgressUpdater
    {
        /// <summary>
        ///     Асинхронно обновляет прогресс выполнения задачи.
        /// </summary>
        /// <param name="reportProgress">Делегат для обновления прогресса.</param>
        /// <param name="estimatedTime">Оценочное время выполнения задачи в секундах.</param>
        /// <param name="isLoadingComplete">Функция, возвращающая true, если задача завершена.</param>
        Task UpdateProgressAsync(Action<double> reportProgress, double estimatedTime, Func<bool> isLoadingComplete);
    }
}