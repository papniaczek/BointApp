using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace BointApp.Converters;

public class PageNameConverter : IValueConverter
{
    public static readonly PageNameConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Sprawdzamy, czy przekazana wartość nie jest nullem
        if (value is not null)
        {
            // Pobieramy nazwę typu obiektu (np. "CityBike")
            // i usuwamy z niej końcówki "ViewModel" lub "Bike"
            return value.GetType().Name.Replace("ViewModel", "").Replace("Bike", "");
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}