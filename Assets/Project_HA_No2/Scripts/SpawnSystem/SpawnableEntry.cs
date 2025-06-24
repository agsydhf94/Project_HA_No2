using UnityEngine;

namespace HA
{
    /// <summary>
    /// Defines spawnable entity parameters for use in a trigger-based spawn system.
    /// Includes ID, position offset, rotation, optional VFX, delay, and lifetime.
    /// </summary>
    [System.Serializable]
    public class SpawnableEntry
    {
        /// <summary>Addressables spawn ID of the object.</summary>
        public string spawnID;

        /// <summary>Offset from the trigger position where the object will be spawned.</summary>
        public Vector3 offset;

        /// <summary>Initial rotation to apply to the spawned object.</summary>
        public Quaternion rotation;

        /// <summary>Optional VFX ID to play at the spawn position.</summary>
        public string vfxID;

        /// <summary>Delay before the object is spawned, in seconds.</summary>
        public float delay;

        /// <summary>Lifetime of the spawned object before auto-despawn. Set to 0 for infinite.</summary>
        public float lifetime;
    }
}
