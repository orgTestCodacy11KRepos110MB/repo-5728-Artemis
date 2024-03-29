﻿namespace Artemis.Storage.Entities.Profile.AdaptionHints;

public class DeviceAdaptionHintEntity : IAdaptionHintEntity
{
    public int DeviceType { get; set; }

    public bool LimitAmount { get; set; }
    public int Skip { get; set; }
    public int Amount { get; set; }
}