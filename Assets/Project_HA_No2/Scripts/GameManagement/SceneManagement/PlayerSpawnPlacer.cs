using UnityEngine;

namespace HA
{
    /// <summary>
    /// Places the player at the spawn point defined by <see cref="WarpContext"/>
    /// after a scene transition. Temporarily disables the CharacterController
    /// to safely apply position and rotation.
    /// </summary>
    public class PlayerSpawnPlacer : SingletonBase<PlayerSpawnPlacer>
    {
        /// <summary>
        /// The player's transform to be positioned at the target spawn point.
        /// </summary>
        public Transform player;


        /// <summary>
        /// Reads the next spawn point ID from <see cref="WarpContext"/> and,
        /// if found, moves the player to that location. Clears the context afterwards.
        /// </summary>
        public void SetSpawnPlayer()
        {
            Debug.Log("PlayerSpawnPlacer Started");
            if (string.IsNullOrEmpty(WarpContext.NextSpawnPointId)) return;

            if (SpawnPoint.TryGetSpawnPoint(WarpContext.NextSpawnPointId, out var spawnPointTransform))
            {
                var characterController = player.gameObject.GetComponent<CharacterController>();

                characterController.enabled = false;
                player.position = spawnPointTransform.position;
                player.rotation = spawnPointTransform.rotation;
                characterController.enabled = true;
            }

            WarpContext.Clear();
        }
    }
}
