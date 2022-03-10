using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MyFemsWpfClient.Converters;

internal class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isVisible = value is bool val && val;

        if("inverse".Equals(parameter as string, StringComparison.OrdinalIgnoreCase))
            isVisible = !isVisible;

        return isVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
