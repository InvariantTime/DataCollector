
using DataCollector.Terminal.App.DTOs;
using System.Globalization;

namespace DataCollector.Terminal.App.Converters
{
    public class ConnectionDTOConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type t, object p, CultureInfo c)
        {
            if (values.Length == 3 &&
                values.All(v => v is string))
            {
                return new ConnectionDTO(
                    (string)values[0],
                    (string)values[1],
                    (string)values[2]);
            }

            return Binding.DoNothing;
        }

        public object[] ConvertBack(object v, Type[] t, object p, CultureInfo c)
            => throw new NotSupportedException();
    }
}
