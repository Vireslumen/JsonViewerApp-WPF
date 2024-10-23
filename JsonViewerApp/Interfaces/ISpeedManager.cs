namespace JsonViewerApp.Interfaces
{
    /// <summary>
    ///     Интерфейс для управления скоростью обработки данных.
    /// </summary>
    public interface ISpeedManager
    {
        /// <summary>
        ///     Загружает сохранённую скорость обработки данных.
        /// </summary>
        /// <returns>Сохранённая скорость.</returns>
        double LoadSpeed();

        /// <summary>
        ///     Сохраняет скорость обработки данных.
        /// </summary>
        /// <param name="speed">Скорость, которую необходимо сохранить.</param>
        void SaveSpeed(double speed);
    }
}