using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Represents a single prefab entry to be pooled.
    /// </summary>
    [System.Serializable]
    public class PoolPrefabEntry
    {
        /// <summary>
        /// Unique identifier used to reference the pooled prefab.
        /// </summary>
        public string key;

        /// <summary>
        /// The prefab (as a Component) that will be pooled.
        /// </summary>              
        public Component prefab;

        /// <summary>
        /// Number of instances to pre-instantiate in the pool.
        /// </summary>        
        public int initialSize;         
    }

    /// <summary>
    /// A ScriptableObject that holds a list of prefab entries for object pooling.
    /// </summary>
    [CreateAssetMenu(menuName = "Object Pool/Prefab Table", fileName = "PoolPrefabTable")]
    public class PoolPrefabTableSO : ScriptableObject
    {
        /// <summary>
        /// List of prefabs and their associated pooling configurations.
        /// </summary>
        public List<PoolPrefabEntry> poolPrefabs = new();
    }
}
