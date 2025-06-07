using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace BointApp.Converters;

public class IndexToVisibilityConverter : IValueConverter
{
    public static readonly IndexToVisibilityConverter Instance = new();
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int index && parameter is string targetIndexStr && int.TryParse(targetIndexStr, out var targetIndex))
        {
            return index == targetIndex;
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}