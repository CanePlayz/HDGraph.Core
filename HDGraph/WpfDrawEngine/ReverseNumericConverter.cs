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
            if (value is float)
                return -1 * (float)value;
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is double)
                return -1 * (double)value;
            if (value is float)
                return -1 * (float)value;
            return Binding.DoNothing;
        }
    }

    /// <summary>
    /// Converter used to divide by 2 a numeric.
    /// </summary>
    internal class DivideBy2NumericConverter : IValueConverter
    {
        public DivideBy2NumericConverter()
            : this(false)
        {
        }

        private int coef;

        public DivideBy2NumericConverter(bool reverse)
        {
            this.coef = reverse ? -1 : 1;
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is double)
                return ((double)value) / 2 * coef;
            if (value is float)
                return ((float)value) / 2 * coef;
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is double)
                return 2 * ((double)value) * coef;
            if (value is float)
                return 2 * ((float)value) * coef;
            return Binding.DoNothing;
        }
    }
}
