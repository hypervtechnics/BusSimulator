using System;
using System.Windows.Data;

namespace BusSimulator.Ui.Converters
{
    public class NegatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is double x ? -x : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is double x ? +x : value;
        }
    }
}
