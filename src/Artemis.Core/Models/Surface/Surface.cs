﻿using System;
using System.Collections.Generic;
using System.Linq;
using Artemis.Core.Extensions;
using Artemis.Storage.Entities;
using RGB.NET.Core;

namespace Artemis.Core.Models.Surface
{
    public class Surface
    {
        internal Surface(RGBSurface rgbSurface, string name, double scale)
        {
            SurfaceEntity = new SurfaceEntity {DeviceEntities = new List<DeviceEntity>()};
            Guid = System.Guid.NewGuid().ToString();

            Name = name;
            Scale = scale;
            RgbSurface = rgbSurface;
            IsActive = false;
            Devices = new List<Device>();

            ApplyToEntity();
        }

        internal Surface(RGBSurface rgbSurface, SurfaceEntity surfaceEntity, double scale)
        {
            RgbSurface = rgbSurface;
            SurfaceEntity = surfaceEntity;
            Scale = scale;
            Guid = surfaceEntity.Guid;

            Name = surfaceEntity.Name;
            IsActive = surfaceEntity.IsActive;
            Devices = new List<Device>();

            if (surfaceEntity.DeviceEntities == null)
                return;

            foreach (var position in surfaceEntity.DeviceEntities)
            {
                var device = rgbSurface.Devices.FirstOrDefault(d => d.GetDeviceHashCode() == position.DeviceHashCode);
                if (device != null)
                    Devices.Add(new Device(device, this, position));
            }
        }

        public RGBSurface RgbSurface { get; }
        public double Scale { get; private set; }
        public string Name { get; set; }
        public bool IsActive { get; internal set; }
        public List<Device> Devices { get; internal set; }

        internal SurfaceEntity SurfaceEntity { get; set; }
        internal string Guid { get; set; }

        internal void ApplyToEntity()
        {
            SurfaceEntity.Guid = Guid;
            SurfaceEntity.Name = Name;
            SurfaceEntity.IsActive = IsActive;
        }

        internal void Destroy()
        {
            SurfaceEntity = null;

            foreach (var deviceConfiguration in Devices)
                deviceConfiguration.Destroy();
            Devices.Clear();
        }

        public void UpdateScale(double value)
        {
            Scale = value;
            foreach (var device in Devices)
                device.CalculateRenderRectangle();

            OnScaleChanged();
        }

        #region Events

        public event EventHandler<EventArgs> ScaleChanged;

        protected virtual void OnScaleChanged()
        {
            ScaleChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}