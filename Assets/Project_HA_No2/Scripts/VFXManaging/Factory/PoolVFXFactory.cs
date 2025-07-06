using System;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// A VFX factory that retrieves visual effects from an object pool.
    /// Implements the IVFXFactory interface to provide pooled VFX components.
    /// </summary>
    public class PoolVFXFactory : IVFXFactory
    {
        private readonly ObjectPool pool;

        /// <summary>
        /// Constructs a PoolVFXFactory using the given object pool.
        /// </summary>
        /// <param name="pool">The object pool used to retrieve VFX instances.</param>
        public PoolVFXFactory(ObjectPool pool)
        {
            this.pool = pool;
        }


        /// <summary>
        /// Loads a VFX component from the pool using the given key, and invokes the callback when loaded.
        /// </summary>
        /// <param name="key">The identifier of the VFX prefab in the pool.</param>
        /// <param name="onLoaded">Callback to receive the loaded component.</param>
        public void LoadFX(string key, Action<Component> onLoaded)
        {
            var component = pool.GetFromPool<Component>(key);
            onLoaded?.Invoke(component);
        }
    }
}
