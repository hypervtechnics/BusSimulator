using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BusSimulator.Ui.Converters
{
    public class IsLastItemConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int count = (int) values[2];

            if (values?.Length == 3 && count > 0)
            {
                System.Windows.Controls.ItemsControl itemsControl = values[0] as System.Windows.Controls.ItemsControl;
                var itemContext = (values[1] as System.Windows.Controls.ContentPresenter)?.DataContext;

                var lastItem = itemsControl.Items[count - 1];

                return Equals(lastItem, itemContext);
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
