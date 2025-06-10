using UnityEngine;

namespace HA
{
    /// <summary>
    /// Initializes object pools at runtime using the entries from a PoolPrefabTableSO.
    /// </summary>
    public class ObjectArchive : MonoBehaviour
    {
        /// <summary>
        /// Reference to the ScriptableObject that defines which prefabs to pool.
        /// </summary>
        [SerializeField] private PoolPrefabTableSO prefabTable;


        /// <summary>
        /// On Awake, registers all prefabs in the pool using the provided table.
        /// </summary>
        private void Awake()
        {
            var pool = ObjectPool.Instance;

            foreach (var entry in prefabTable.poolPrefabs)
            {
                // Skip invalid entries (missing key or prefab)
                if (entry.prefab == null || string.IsNullOrEmpty(entry.key))
                {
                    Debug.LogWarning($"[ObjectArchive] Invalid pool entry: {entry.key}");
                    continue;
                }

                // Create a pool for the specified prefab
                pool.CreatePool(entry.key, entry.prefab, entry.initialSize);
            }
        }
    }
}
