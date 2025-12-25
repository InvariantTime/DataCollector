using DataCollector.Domain;
using System.Globalization;

namespace DataCollector.Terminal.App.Converters;

public class RoleColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not UserRoles role)
            return Binding.DoNothing;

        return role switch
        {
            UserRoles.Publisher => Colors.Yellow,
            UserRoles.Admin => Colors.Red,
            _ => Colors.White
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
