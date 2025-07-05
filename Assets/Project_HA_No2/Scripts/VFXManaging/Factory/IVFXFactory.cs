using System;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Defines an interface for loading visual effect components (VFX) by key.
    /// The result is delivered via callback to support both sync and async loading strategies.
    /// </summary>
    public interface IVFXFactory
    {
        /// <param name="key">The unique identifier for the VFX asset.</param>
        /// <param name="onLoaded">Callback invoked with the loaded VFX component.</param>
        void LoadFX(string key, Action<Component> onLoaded);
    }
}
