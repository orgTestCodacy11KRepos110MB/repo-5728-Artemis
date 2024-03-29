﻿using System;

namespace Artemis.Core;

/// <summary>
///     Represents progress on a plugin prerequisite action
/// </summary>
public class PrerequisiteActionProgress : CorePropertyChanged, IProgress<(long, long)>
{
    private long _current;
    private DateTime _lastReport;
    private long _lastReportValue;
    private double _percentage;
    private double _progressPerSecond;
    private long _total;

    /// <summary>
    ///     The current amount
    /// </summary>
    public long Current
    {
        get => _current;
        set => SetAndNotify(ref _current, value);
    }

    /// <summary>
    ///     The total amount
    /// </summary>
    public long Total
    {
        get => _total;
        set => SetAndNotify(ref _total, value);
    }

    /// <summary>
    ///     The percentage
    /// </summary>
    public double Percentage
    {
        get => _percentage;
        set => SetAndNotify(ref _percentage, value);
    }

    /// <summary>
    ///     Gets or sets the progress per second
    /// </summary>
    public double ProgressPerSecond
    {
        get => _progressPerSecond;
        set => SetAndNotify(ref _progressPerSecond, value);
    }

    /// <summary>
    ///     Occurs when progress has been reported
    /// </summary>
    public event EventHandler? ProgressReported;

    /// <summary>
    ///     Invokes the <see cref="ProgressReported" /> event
    /// </summary>
    protected virtual void OnProgressReported()
    {
        ProgressReported?.Invoke(this, EventArgs.Empty);
    }

    #region Implementation of IProgress<in (long, long)>

    /// <inheritdoc />
    public void Report((long, long) value)
    {
        (long newCurrent, long newTotal) = value;

        TimeSpan timePassed = DateTime.Now - _lastReport;
        if (timePassed >= TimeSpan.FromSeconds(1))
        {
            ProgressPerSecond = Math.Max(0, Math.Round(1.0 / timePassed.TotalSeconds * (newCurrent - _lastReportValue), 2));
            _lastReportValue = newCurrent;
            _lastReport = DateTime.Now;
        }

        Current = newCurrent;
        Total = newTotal;
        Percentage = Math.Round((double) Current / Total * 100.0, 2);

        OnProgressReported();
    }

    #endregion
}