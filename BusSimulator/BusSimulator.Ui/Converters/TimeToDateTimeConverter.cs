using BusSimulator.Core.Models;

using System;
using System.Globalization;
using System.Windows.Data;

namespace BusSimulator.Ui.Converters
{
    public class TimeToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(DateTime?) && value is Time time)
            {
                return time.ToDateTime();
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Time) && value is DateTime?)
            {
                var time = value as DateTime?;

                if (time.HasValue)
                {
                    return new Time(time.Value);
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return value;
            }
        }
    }
}
