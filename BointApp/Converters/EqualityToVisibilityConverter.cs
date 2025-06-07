using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace BointApp.Converters;

public class EqualityToVisibilityConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count < 2 || values[0] is null || values[1] is null)
        {
            return false;
        }
        return values[0].Equals(values[1]);
    }
}