using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Linq;

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
                return System.Convert.ChangeType(-1 * (double)value, targetType);
            if (value is float)
                return System.Convert.ChangeType(-1 * (float)value, targetType);
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value is double)
                return System.Convert.ChangeType(-1 * (double)value, targetType);
            if (value is float)
                return System.Convert.ChangeType(-1 * (float)value, targetType);
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
            int multiplier = 1;
            if (parameter != null && parameter is string)
                int.TryParse((string)parameter, out multiplier);

            if (value is double)
                return System.Convert.ChangeType(((double)value) / 2 * coef * multiplier, targetType);
            if (value is float)
                return System.Convert.ChangeType(((float)value) / 2 * coef * multiplier, targetType);
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            //if (value is double)
            //    return System.Convert.ChangeType(2 * ((double)value) * coef, targetType);
            //if (value is float)
            //    return System.Convert.ChangeType(2 * ((float)value) * coef, targetType);
            //return Binding.DoNothing;
            throw new NotImplementedException();
        }
    }



    /// <summary>
    /// Converter used to return the average a numeric values.
    /// </summary>
    internal class MakeAverageNumericConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0)
                return 0;
            if (values[0] is double)
                return System.Convert.ChangeType(values.Average(x => System.Convert.ToDouble(x)), targetType);
            if (values[0] is float)
                return System.Convert.ChangeType(values.Average(x => System.Convert.ToSingle(x)), targetType);
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

   
}
