using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace HDGraph.WpfDrawEngine
{
    /// <summary>
    /// Converter used to reverse (multiply by -1) a numeric.
    /// </summary>
    internal class ReverseNumericConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is double)
                return -1 * (double)value;
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is double)
                return -1 * (double)value;
            return Binding.DoNothing;
        }
    }
}
