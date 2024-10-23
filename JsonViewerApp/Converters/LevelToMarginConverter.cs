using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JsonViewerApp.Converters
{
    /// <summary>
    ///     Конвертер, который преобразует уровень вложенности элемента JSON,
    ///     а также ширину текстовых блоков (имя и разделитель) и ширину полосы прокрутки в отступ (Margin).
    /// </summary>
    public class LevelToMarginConverter : IMultiValueConverter
    {
        private const double BaseMargin = 33; // Базовый отступ
        private const double LevelMarginIncrement = 19; // Увеличение отступа на уровень
        private const double ScrollBarWidth = 18; // Ширина полосы прокрутки

        /// <summary>
        ///     Преобразует уровень вложенности, ширину текстовых блоков имени и разделителя в отступ для элемента.
        /// </summary>
        /// <param name="values">Массив значений: уровень вложенности, ширина имени, ширина разделителя.</param>
        /// <param name="targetType">Целевой тип данных (не используется).</param>
        /// <param name="parameter">Параметры привязки (не используется).</param>
        /// <param name="culture">Культура данных (не используется).</param>
        /// <returns>Отступ (Margin) для элемента, рассчитанный на основе входных данных.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверка, что переданы все необходимые значения и они правильного типа
            if (values.Length != 3 || values[0] is not int level || values[1] is not double nameWidth || values[2] is not double separatorWidth)
                return new Thickness(0);

            // Расчет отступа с учетом уровня, ширины имени, разделителя и полосы прокрутки
            var width = BaseMargin + level * LevelMarginIncrement + nameWidth + separatorWidth + ScrollBarWidth;
            return new Thickness(0, 0, width, 0);
        }

        public object[]? ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Обратное преобразование не требуется для данного конвертера, возвращаем пустой массив
            return null;
        }
    }
}