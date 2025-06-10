using UnityEngine;
using UnityEngine.VFX;

namespace HA
{
    /// <summary>
    /// Initializes and registers all VFX prefabs defined in the VFXPoolTableSO into the object pool.
    /// Inherits from SingletonBase to ensure only one instance exists.
    /// </summary>
    public class VFXArchive : SingletonBase<VFXArchive>
    {
        /// <summary>
        /// Reference to the VFX pool table that contains all VFX entries to preload.
        /// </summary>
        [SerializeField] private VFXPoolTableSO tableSO;

        /// <summary>
        /// Called during the Awake phase; initializes the VFX object pools.
        /// </summary>
        public override void Awake()
        {
            base.Awake();

            if (tableSO == null)
            {
                Debug.LogError("[VFXArchive] VFXTableSO is not assigned.");
                return;
            }

            var pool = ObjectPool.Instance;
            
            // Iterate through all VFX entries and register them into the object pool
            foreach (var entry in tableSO.entries)
            {
                if (entry.prefab != null)
                    pool.CreatePool(entry.key, entry.prefab, entry.poolSize);
                else
                    Debug.LogWarning($"[VFXArchive] Missing prefab for key: {entry.key}");
            }
        }
    }
}
