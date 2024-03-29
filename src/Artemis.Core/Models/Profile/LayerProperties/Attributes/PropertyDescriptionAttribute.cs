﻿using System;

namespace Artemis.Core;

/// <summary>
///     Represents a description attribute used to decorate layer properties
/// </summary>
public class PropertyDescriptionAttribute : Attribute
{
    /// <summary>
    ///     The identifier of this property used for storage, if not set one will be generated property name in code
    /// </summary>
    public string? Identifier { get; set; }

    /// <summary>
    ///     The user-friendly name for this property, shown in the UI
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     The user-friendly description for this property, shown in the UI
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Input prefix to show before input elements in the UI
    /// </summary>
    public string? InputPrefix { get; set; }

    /// <summary>
    ///     Input affix to show behind input elements in the UI
    /// </summary>
    public string? InputAffix { get; set; }

    /// <summary>
    ///     The input drag step size, used in the UI
    /// </summary>
    public float InputStepSize { get; set; }

    /// <summary>
    ///     Minimum input value, only enforced in the UI
    /// </summary>
    public object? MinInputValue { get; set; }

    /// <summary>
    ///     Maximum input value, only enforced in the UI
    /// </summary>
    public object? MaxInputValue { get; set; }

    /// <summary>
    ///     Whether or not keyframes are always disabled
    /// </summary>
    public bool DisableKeyframes { get; set; }
}