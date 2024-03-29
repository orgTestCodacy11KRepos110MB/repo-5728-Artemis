﻿namespace Artemis.Core;

/// <inheritdoc />
public class BoolLayerProperty : LayerProperty<bool>
{
    internal BoolLayerProperty()
    {
    }

    /// <summary>
    ///     Implicitly converts an <see cref="BoolLayerProperty" /> to a <see cref="bool" />
    /// </summary>
    public static implicit operator bool(BoolLayerProperty p)
    {
        return p.CurrentValue;
    }

    /// <inheritdoc />
    protected override void OnInitialize()
    {
        KeyframesSupported = false;
        DataBinding.RegisterDataBindingProperty(() => CurrentValue, value => CurrentValue = value, "Value");
    }

    /// <inheritdoc />
    protected override void UpdateCurrentValue(float keyframeProgress, float keyframeProgressEased)
    {
        throw new ArtemisCoreException("Boolean properties do not support keyframes.");
    }
}