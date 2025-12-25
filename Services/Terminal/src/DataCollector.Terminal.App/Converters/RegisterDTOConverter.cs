using DataCollector.Terminal.App.DTOs;
using System.Globalization;

namespace DataCollector.Terminal.App.Converters;

public class RegisterDTOConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2 && values.All(x => x is string) == false)
            return Binding.DoNothing;

        return new RegisterDTO((string)values[0], (string)values[1]);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
