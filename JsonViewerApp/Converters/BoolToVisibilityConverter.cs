using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JsonViewerApp.Converters;

/// <summary>
///     Конвертер, преобразующий значение типа <see cref="bool" /> в <see cref="Visibility" />.
///     Используется для изменения видимости элементов на основе логического значения.
/// </summary>
public class BoolToVisibilityConverter : IValueConverter
{
    /// <summary>
    ///     Преобразует значение <see cref="bool" /> в <see cref="Visibility" />.
    /// </summary>
    /// <param name="value">Значение типа <see cref="bool" />, которое нужно преобразовать.</param>
    /// <param name="targetType">Целевой тип данных (не используется).</param>
    /// <param name="parameter">Дополнительный параметр (не используется).</param>
    /// <param name="culture">Культура данных (не используется).</param>
    /// <returns>
    ///     <see cref="Visibility.Visible" />, если <paramref name="value" /> равно true; иначе
    ///     <see cref="Visibility.Collapsed" />.
    /// </returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue) return boolValue ? Visibility.Visible : Visibility.Collapsed;
        return Visibility.Collapsed;
    }

    /// <summary>
    ///     Преобразует значение <see cref="Visibility" /> обратно в <see cref="bool" />.
    /// </summary>
    /// <param name="value">Значение типа <see cref="Visibility" />, которое нужно преобразовать обратно.</param>
    /// <param name="targetType">Целевой тип данных (не используется).</param>
    /// <param name="parameter">Дополнительный параметр (не используется).</param>
    /// <param name="culture">Культура данных (не используется).</param>
    /// <returns>True, если <paramref name="value" /> равно <see cref="Visibility.Visible" />; иначе false.</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Visibility visibility) return visibility == Visibility.Visible;
        return false;
    }
}