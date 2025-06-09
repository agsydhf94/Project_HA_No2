using UnityEngine;

namespace HA
{
    /// <summary>
    /// Defines a contract for spawning objects in the game world.
    /// Implementations determine how and where objects are instantiated (e.g., pooling, addressables).
    /// </summary>
    public interface IObjectSpawner
    {
        /// <summary>
        /// Spawns an object based on the given key and transform data.
        /// May use different loading strategies depending on the source type.
        /// </summary>
        /// <param name="key">Unique key identifying the object (e.g., prefab name).</param>
        /// <param name="position">Spawn world position.</param>
        /// <param name="rotation">Spawn rotation.</param>
        /// <param name="parent">Optional parent transform to attach the object to.</param>
        /// <param name="sourceType">The object source (e.g., Object Pool or Addressables).</param>
        /// <returns>Component instance if loaded synchronously, or null if loading asynchronously.</returns>
        Component Spawn(string key, Vector3 position, Quaternion rotation, Transform parent, ObjectSourceType sourceType);
    }
}

