using System;
using System.Globalization;
using System.Windows.Data;

namespace JsonViewerApp.Converters;

/// <summary>
///     Конвертер для преобразования булевого значения в текстовое представление для кнопки раскрытия/сворачивания дерева.
/// </summary>
public class BoolToExpandCollapseTextConverter : IValueConverter
{
    /// <summary>
    ///     Преобразует булевое значение в текст "Раскрыть всё" или "Свернуть всё" в зависимости от значения.
    /// </summary>
    /// <param name="value">Значение булевого типа (true или false), указывающее состояние дерева (раскрыто или свернуто).</param>
    /// <param name="targetType">Тип целевого свойства привязки. Этот параметр не используется.</param>
    /// <param name="parameter">Необязательный параметр. Не используется в данном контексте.</param>
    /// <param name="culture">Культура, используемая в преобразовании.</param>
    /// <returns>Строка "Свернуть всё", если дерево раскрыто (true), или "Раскрыть всё", если свернуто (false).</returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Проверяем на null и корректное приведение типов
        if (value is bool isExpanded) return isExpanded ? "Свернуть всё" : "Раскрыть всё";

        // Если значение null или не является bool, возвращаем "Раскрыть всё" по умолчанию
        return "Раскрыть всё";
    }

    /// <summary>
    ///     Преобразование строки обратно в булевое значение не поддерживается, поэтому возвращаем значение по умолчанию.
    /// </summary>
    /// <param name="value">Значение, переданное обратно из интерфейса (не используется).</param>
    /// <param name="targetType">Тип целевого свойства привязки (не используется).</param>
    /// <param name="parameter">Необязательный параметр (не используется).</param>
    /// <param name="culture">Культура, используемая в преобразовании (не используется).</param>
    /// <returns>Значение по умолчанию — false.</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Поскольку обратное преобразование не используется, возвращаем false по умолчанию
        return false;
    }
}