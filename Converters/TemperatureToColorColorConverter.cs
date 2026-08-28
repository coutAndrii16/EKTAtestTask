using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EKTAtestTask.Converters;


public class TemperatureToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double temp) return Brushes.Gray;
        return temp switch
        {
            < 30 => new SolidColorBrush(Color.FromRgb(70, 130, 230)),
            < 55 => new SolidColorBrush(Color.FromRgb(60, 200, 100)),
            < 75 => new SolidColorBrush(Color.FromRgb(255, 180, 40)),
            _    => new SolidColorBrush(Color.FromRgb(220, 50, 50)),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}