﻿using System.Diagnostics.CodeAnalysis;
using Artemis.Core.Modules;

namespace Artemis.UI.Shared.DataModelVisualization;

/// <summary>
///     Represents a <see cref="DataModel" /> display view model
/// </summary>
/// <typeparam name="T">The type of the data model</typeparam>
public abstract class DataModelDisplayViewModel<T> : DataModelDisplayViewModel
{
    [AllowNull] private T _displayValue;

    /// <summary>
    ///     Gets or sets value that the view model must display
    /// </summary>
    [AllowNull]
    public T DisplayValue
    {
        get => _displayValue;
        set
        {
            if (Equals(value, _displayValue)) return;
            RaiseAndSetIfChanged(ref _displayValue, value);
            OnDisplayValueUpdated();
        }
    }

    internal override object InternalGuard => new();

    /// <inheritdoc />
    public override void UpdateValue(object? model)
    {
        DisplayValue = model is T value ? value : default;
    }

    /// <summary>
    ///     Occurs when the display value is updated
    /// </summary>
    protected virtual void OnDisplayValueUpdated()
    {
    }
}

/// <summary>
///     For internal use only, implement <see cref="DataModelDisplayViewModel{T}" /> instead.
/// </summary>
public abstract class DataModelDisplayViewModel : ViewModelBase
{
    private DataModelPropertyAttribute? _propertyDescription;

    /// <summary>
    ///     Gets the property description of this value
    /// </summary>
    public DataModelPropertyAttribute? PropertyDescription
    {
        get => _propertyDescription;
        internal set => RaiseAndSetIfChanged(ref _propertyDescription, value);
    }

    /// <summary>
    ///     Prevents this type being implemented directly, implement <see cref="DataModelDisplayViewModel{T}" /> instead.
    /// </summary>
    internal abstract object InternalGuard { get; }

    /// <summary>
    ///     Updates the display value
    /// </summary>
    /// <param name="model">The value to set</param>
    public abstract void UpdateValue(object? model);
}