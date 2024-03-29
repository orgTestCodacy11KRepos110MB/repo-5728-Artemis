﻿using System;
using System.Globalization;
using System.Linq;
using Artemis.Core;
using Avalonia.Data.Converters;
using Avalonia.Media;
using SkiaSharp;

namespace Artemis.UI.Shared.Converters;

/// <summary>
///     Converts  <see cref="T:Artemis.Core.ColorGradient" /> into a <see cref="T:Avalonia.Media.GradientStops" />.
/// </summary>
public class ColorGradientToGradientStopsConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ColorGradient? colorGradient = value as ColorGradient;
        GradientStops collection = new();
        if (colorGradient == null)
            return collection;

        foreach (ColorGradientStop c in colorGradient.OrderBy(s => s.Position))
            collection.Add(new GradientStop(Color.FromArgb(c.Color.Alpha, c.Color.Red, c.Color.Green, c.Color.Blue), c.Position));
        return collection;
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        GradientStops? collection = value as GradientStops;
        ColorGradient colorGradients = new();
        if (collection == null)
            return colorGradients;

        foreach (GradientStop c in collection.OrderBy(s => s.Offset))
            colorGradients.Add(new ColorGradientStop(new SKColor(c.Color.R, c.Color.G, c.Color.B, c.Color.A), (float) c.Offset));
        return colorGradients;
    }
}