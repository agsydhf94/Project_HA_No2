using UnityEngine;

namespace HA
{
    /// <summary>
    /// Marks a spawn location in the scene that can be looked up by an ID.
    /// </summary>
    public class SpawnPoint : MonoBehaviour
    {
        /// <summary>
        /// Identifier used to find this spawn point at runtime.
        /// </summary>
        [SerializeField] private string spawnPointId;

        /// <summary>
        /// Tries to find a <see cref="SpawnPoint"/> with the given ID in the active scene.
        /// </summary>
        /// <param name="id">The spawn point identifier to search for.</param>
        /// <param name="transform">The transform of the found spawn point, if any.</param>
        /// <returns>True if a spawn point was found; otherwise false.</returns>
        public static bool TryGetSpawnPoint(string id, out Transform transform)
        {
            foreach (var sp in GameObject.FindObjectsOfType<SpawnPoint>())
            {
                if (sp.spawnPointId == id)
                {
                    transform = sp.transform; return true;
                }
            }
            transform = null;
            return false;
        }
    }
}
