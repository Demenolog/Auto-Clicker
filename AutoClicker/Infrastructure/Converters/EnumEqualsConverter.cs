using System;
using System.Globalization;
using System.Windows.Data;

namespace AutoClicker.Infrastructure.Converters
{
    internal sealed class EnumEqualsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || parameter is null)
            {
                return false;
            }

            var parameterValue = parameter.ToString();

            return parameterValue is not null && value.ToString() == parameterValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter is not null && targetType.IsEnum)
            {
                return Enum.Parse(targetType, parameter.ToString());
            }

            return Binding.DoNothing;
        }
    }
}
