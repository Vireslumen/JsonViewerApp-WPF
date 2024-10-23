using System.ComponentModel;

namespace JsonViewerApp.ViewModels
{
    /// <summary>
    ///     Базовый класс для всех ViewModel в приложении, реализующий интерфейс INotifyPropertyChanged.
    ///     Обеспечивает механизм уведомления об изменениях свойств.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        ///     Событие, вызываемое при изменении свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        ///     Вызывает событие изменения свойства для указанного имени свойства.
        /// </summary>
        /// <param name="propertyName">Имя изменённого свойства.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}