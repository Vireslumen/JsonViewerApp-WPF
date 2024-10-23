using System;
using System.Diagnostics;
using System.Threading.Tasks;
using JsonViewerApp.Interfaces;

namespace JsonViewerApp.Services;

/// <summary>
///     Класс для обновления прогресса выполнения длительных задач.
/// </summary>
public class ProgressUpdater(ILoggerService loggerService) : IProgressUpdater
{
    private const double MaxProgressBeforeSlowdown = 90; // Максимальный прогресс перед замедлением
    private const double SlowdownFactor = 0.0001; // Коэффициент для замедления прогресса
    private const int ProgressUpdateIntervalMs = 100; // Интервал обновления прогресса в миллисекундах
    private const double FinalProgress = 100; // Максимальное значение прогресса

    /// <inheritdoc />
    public async Task UpdateProgressAsync(Action<double> reportProgress, double estimatedTime, Func<bool> isLoadingComplete)
    {
        try
        {
            if (estimatedTime <= 0)
            {
                reportProgress(FinalProgress); // Если время равно 0, прогресс сразу достигает 100%
                return;
            }

            double elapsedTime = 0;
            var progressStopwatch = new Stopwatch();
            progressStopwatch.Start();
            double progressPercentage = 0;

            while (elapsedTime < estimatedTime)
            {
                await Task.Delay(ProgressUpdateIntervalMs); // Задержка между обновлениями прогресса
                elapsedTime = progressStopwatch.Elapsed.TotalSeconds;

                // Проверка, завершена ли задача досрочно
                if (isLoadingComplete())
                {
                    reportProgress(FinalProgress); // Установка прогресса на 100%
                    break;
                }

                // Обновление прогресса до 90%
                if (progressPercentage < MaxProgressBeforeSlowdown)
                {
                    progressPercentage = Math.Min(elapsedTime / estimatedTime * MaxProgressBeforeSlowdown, MaxProgressBeforeSlowdown);
                }
                else
                {
                    // Замедленный рост прогресса от 90% до 100%
                    var remainingTime = estimatedTime - elapsedTime;
                    var tenPercentTime = estimatedTime * SlowdownFactor;

                    if (tenPercentTime > 0)
                    {
                        var slowProgress = 10 * (1 - remainingTime / tenPercentTime);
                        progressPercentage = MaxProgressBeforeSlowdown + slowProgress;
                    }
                    else
                    {
                        progressPercentage = FinalProgress;
                    }
                }

                reportProgress(Math.Min(progressPercentage, FinalProgress));
            }

            progressStopwatch.Stop();
        }
        catch (Exception ex)
        {
            loggerService.Error(ex, "Ошибка при обновлении прогресса.");
        }
    }
}