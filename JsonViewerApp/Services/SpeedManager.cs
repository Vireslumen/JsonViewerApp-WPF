using System;
using System.IO;
using JsonViewerApp.Interfaces;
using Newtonsoft.Json;

namespace JsonViewerApp.Services;

/// <summary>
///     Класс для управления скоростью обработки данных и сохранения/загрузки этой информации из файла.
/// </summary>
public class SpeedManager(ILoggerService loggerService) : ISpeedManager
{
    private const double DefaultSpeed = 1.0;
    private const string SpeedDataFilePath = "speed_data.json";

    /// <inheritdoc />
    public double LoadSpeed()
    {
        try
        {
            if (!File.Exists(SpeedDataFilePath))
            {
                loggerService.Information("Файл скорости не найден, возвращается скорость по умолчанию.");
                return DefaultSpeed;
            }

            // Чтение файла и десериализация JSON в объект
            var json = File.ReadAllText(SpeedDataFilePath);
            var speedData = JsonConvert.DeserializeObject<dynamic>(json);
            if (speedData == null) return DefaultSpeed;
            var loadedSpeed = (double) speedData.LastSpeed > 0 ? (double) speedData.LastSpeed : DefaultSpeed;
            return loadedSpeed;
        }
        catch (Exception ex)
        {
            loggerService.Error(ex, "Ошибка при загрузке скорости из файла.");
            return DefaultSpeed; // Возврат значения по умолчанию при ошибке
        }
    }

    /// <inheritdoc />
    public void SaveSpeed(double speed)
    {
        try
        {
            var speedData = new {LastSpeed = speed}; // Объект для сохранения
            // Сериализация объекта в JSON и запись в файл
            var json = JsonConvert.SerializeObject(speedData, Formatting.Indented);
            File.WriteAllText(SpeedDataFilePath, json);
        }
        catch (Exception ex)
        {
            loggerService.Error(ex, "Ошибка при сохранении скорости в файл.");
        }
    }
}