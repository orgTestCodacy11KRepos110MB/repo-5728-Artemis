﻿using System;
using Avalonia.Win32;

namespace Artemis.UI.Windows.Providers.Input;

public class SpongeWindow : WindowImpl
{
    public event EventHandler<SpongeWindowEventArgs>? WndProcCalled;

    #region Overrides of WindowImpl

    /// <inheritdoc />
    protected override IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        OnWndProcCalled(new SpongeWindowEventArgs(hWnd, msg, wParam, lParam));
        return base.WndProc(hWnd, msg, wParam, lParam);
    }

    #endregion

    protected virtual void OnWndProcCalled(SpongeWindowEventArgs e)
    {
        WndProcCalled?.Invoke(this, e);
    }
}