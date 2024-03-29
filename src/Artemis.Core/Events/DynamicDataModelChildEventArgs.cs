﻿using System;
using Artemis.Core.Modules;

namespace Artemis.Core;

/// <summary>
///     Provides data about dynamic data model child related events
/// </summary>
public class DynamicDataModelChildEventArgs : EventArgs
{
    internal DynamicDataModelChildEventArgs(DynamicChild dynamicChild, string key)
    {
        DynamicChild = dynamicChild;
        Key = key;
    }

    /// <summary>
    ///     Gets the dynamic data model child
    /// </summary>
    public DynamicChild DynamicChild { get; }

    /// <summary>
    ///     Gets the key of the dynamic data model on the parent <see cref="DataModel" />
    /// </summary>
    public string Key { get; }
}