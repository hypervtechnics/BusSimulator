using BusSimulator.Ui.Logic.Models;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace BusSimulator.Ui.Converters
{
    public class MatrixColorConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MatrixColorType colorType)
            {
                try
                {
                    return Application.Current.FindResource($"LineDisplayForeground{Enum.GetName(typeof(MatrixColorType), colorType)}Brush") as SolidColorBrush;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
