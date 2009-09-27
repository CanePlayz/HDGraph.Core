using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using HDGraph.Interfaces.DrawEngines;
using System.Globalization;
using HDGraph.Interfaces.ScanEngines;

namespace HDGraph.WpfDrawEngine
{
    /// <summary>
    /// Converter used to return the average a numeric values.
    /// </summary>
    internal class VisibilityFromAnglesConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //System.Windows.Visibility.
            if (values == null || values.Length != 2)
                return Visibility.Visible;

            if (!(values[0] is float))
                return Visibility.Visible;
            float minAngle = (float)values[0];
            float currentAngle = (float)values[1];

            if (currentAngle >= minAngle)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    /// <summary>
    /// Converter used to format text depending of a DrawOptions instance.
    /// </summary>
    internal class ArcLabelFromDrawOptionsConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2)
                return String.Empty;

            IDirectoryNode node = values[0] as IDirectoryNode;
            DrawOptions options = values[1] as DrawOptions;

            if (node == null
                || options == null)
                return String.Empty;

            if (options.ShowSize)
                return node.Name + Environment.NewLine + node.HumanReadableTotalSize;
            else
                return node.Name;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
