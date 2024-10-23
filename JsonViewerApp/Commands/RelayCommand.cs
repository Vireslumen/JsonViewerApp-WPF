using System;
using System.Windows.Input;

namespace JsonViewerApp.Commands;

/// <summary>
///     Реализация команды, которая принимает метод для выполнения и метод для проверки возможности выполнения.
///     Используется для команд без параметров.
/// </summary>
public class RelayCommand(Action execute, Func<bool>? canExecute = null) : ICommand
{
    private readonly Action _execute = execute ?? throw new ArgumentNullException(nameof(execute));

    /// <summary>
    ///     Определяет, может ли команда выполняться.
    /// </summary>
    /// <param name="parameter">Параметр, передаваемый в команду. Не используется в данной реализации.</param>
    /// <returns>true, если команда может быть выполнена; иначе false.</returns>
    public bool CanExecute(object parameter)
    {
        return canExecute?.Invoke() ?? true;
    }

    /// <summary>
    ///     Выполняет команду.
    /// </summary>
    /// <param name="parameter">Параметр, передаваемый в команду. Не используется в данной реализации.</param>
    public void Execute(object parameter)
    {
        _execute();
    }

    /// <summary>
    ///     Событие, вызываемое при изменении состояния выполнения команды.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}

/// <summary>
///     Реализация команды, принимающей параметризированное действие и метод для проверки возможности выполнения.
/// </summary>
/// <typeparam name="T">Тип параметра команды.</typeparam>
public class RelayCommand<T>(Action<T> execute, Func<T, bool>? canExecute = null) : ICommand
{
    private readonly Action<T> _execute = execute ?? throw new ArgumentNullException(nameof(execute));

    /// <summary>
    ///     Определяет, может ли команда выполняться с переданным параметром.
    /// </summary>
    /// <param name="parameter">Параметр, передаваемый в команду.</param>
    /// <returns>true, если команда может быть выполнена с параметром; иначе false.</returns>
    public bool CanExecute(object? parameter)
    {
        if (parameter == null || typeof(T).IsValueType)
            return false;

        return canExecute == null || canExecute((T) parameter);
    }

    /// <summary>
    ///     Выполняет команду с параметром.
    /// </summary>
    /// <param name="parameter">Параметр, передаваемый в команду.</param>
    public void Execute(object parameter)
    {
        _execute((T) parameter);
    }

    /// <summary>
    ///     Событие, вызываемое при изменении состояния выполнения команды.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}