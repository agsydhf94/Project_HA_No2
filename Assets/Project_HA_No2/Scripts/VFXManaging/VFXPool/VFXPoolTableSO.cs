using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Represents a single VFX prefab entry to be used with the object pool.
    /// </summary>
    [System.Serializable]
    public class VFXEntry
    {
        /// <summary>
        /// Unique key to identify this VFX effect in the pool.
        /// </summary>
        public string key;

        /// <summary>
        /// The prefab (usually a VFX component) to instantiate and pool.
        /// </summary>
        public Component prefab;

        /// <summary>
        /// The initial number of instances to pool for this VFX.
        /// </summary>
        public int poolSize;
    }

    /// <summary>
    /// ScriptableObject containing a list of VFX entries to be pooled at runtime.
    /// </summary>
    [CreateAssetMenu(menuName = "Object Pool/Prefab Table", fileName = "VFXPoolPrefabTable")]
    public class VFXPoolTableSO : ScriptableObject
    {
        /// <summary>
        /// List of VFX prefabs and their associated keys and pool sizes.
        /// </summary>
        public List<VFXEntry> entries = new();
    }
}
