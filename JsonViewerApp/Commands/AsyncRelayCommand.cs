using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JsonViewerApp.Commands;

/// <summary>
///     Реализация асинхронной команды, которая принимает асинхронный метод для выполнения и метод для проверки возможности
///     выполнения.
///     Используется для асинхронных команд без параметров.
/// </summary>
public class AsyncRelayCommand : ICommand
{
    private readonly Func<bool>? _canExecute; // Метод для проверки возможности выполнения команды
    private readonly Func<Task> _execute; // Асинхронное действие, которое выполняется командой
    private bool _isExecuting; // Флаг, указывающий, выполняется ли команда в данный момент

    /// <summary>
    ///     Инициализирует новый экземпляр команды с асинхронным методом для выполнения и необязательным методом проверки
    ///     выполнения.
    /// </summary>
    /// <param name="execute">Асинхронный метод для выполнения команды.</param>
    /// <param name="canExecute">Метод, определяющий, может ли команда быть выполнена.</param>
    public AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    ///     Определяет, может ли команда выполняться. Команда не может выполняться, если она уже выполняется.
    /// </summary>
    /// <param name="parameter">Параметр, передаваемый в команду. Не используется в данной реализации.</param>
    /// <returns>true, если команда может быть выполнена; иначе false.</returns>
    public bool CanExecute(object? parameter)
    {
        return !_isExecuting && (_canExecute?.Invoke() ?? true);
    }

    /// <summary>
    ///     Выполняет асинхронное действие команды.
    /// </summary>
    /// <param name="parameter">Параметр, передаваемый в команду. Не используется в данной реализации.</param>
    public async void Execute(object? parameter)
    {
        if (!CanExecute(parameter)) return; // Проверка возможности выполнения команды

        _isExecuting = true; // Устанавливаем флаг выполнения
        OnCanExecuteChanged(); // Уведомление об изменении состояния команды

        try
        {
            await _execute(); // Выполнение асинхронного действия
        }
        finally
        {
            _isExecuting = false; // Сбрасываем флаг после завершения выполнения
            OnCanExecuteChanged(); // Уведомление об изменении состояния команды
        }
    }

    /// <summary>
    ///     Событие, вызываемое при изменении состояния выполнения команды.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    ///     Метод для вызова события <see cref="CanExecuteChanged" /> и уведомления о возможности выполнения команды.
    /// </summary>
    protected virtual void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}