using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace BointApp.Converters;

public class PageNameConverter : IValueConverter
{
    public static readonly PageNameConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not null)
        {
            return value.GetType().Name.Replace("ViewModel", "").Replace("Bike", "");
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}