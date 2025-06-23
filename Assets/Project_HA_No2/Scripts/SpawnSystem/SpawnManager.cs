using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Manages spawning and despawning of game objects via the EventBus system.
    /// Listens for SpawnRequest events and handles VFX playback, delayed spawning,
    /// and timed despawning of entities using Addressables.
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
        /// <summary>
        /// Subscribes to spawn events when enabled.
        /// </summary>
        private void OnEnable()
        {
            EventBus.Instance.Subscribe<GameEvent.SpawnRequest>(HandleRequest);
        }

        /// <summary>
        /// Unsubscribes from spawn events when disabled.
        /// </summary>
        private void OnDisable()
        {
            EventBus.Instance.Unsubscribe<GameEvent.SpawnRequest>(HandleRequest);
        }


        /// <summary>
        /// Handles the incoming spawn request event.
        /// </summary>
        /// <param name="request">Spawn request containing ID, position, delay, VFX, and lifetime.</param>
        private void HandleRequest(GameEvent.SpawnRequest request)
        {
            SpawnWithOptions(request).Forget();
        }


        /// <summary>
        /// Spawns an object according to request parameters.
        /// Optionally plays VFX, applies delay before spawn, and schedules auto-despawn if lifetime is set.
        /// </summary>
        /// <param name="request">Detailed spawn instructions.</param>
        private async UniTask SpawnWithOptions(GameEvent.SpawnRequest request)
        {
            // Optional VFX before actual spawn
            if (!string.IsNullOrEmpty(request.vfxID))
            {
                VFXManager.Instance.PlayEffect(
                    request.vfxID,
                    request.position,
                    Quaternion.identity,
                    null,
                    request.delay + 1f,     // Ensure the VFX lasts longer than the spawned object to avoid early disappearance
                    VFXSourceType.Addressables
                );
            }

            // Optional delay before spawning
            if (request.delay > 0f)
                await UniTask.Delay(System.TimeSpan.FromSeconds(request.delay));

            var spawned = ObjectManager.Instance.Spawn(
                request.spawnID,
                request.position,
                request.rotation,
                null,
                ObjectSourceType.Addressables
            );

            // Schedule auto-despawn if lifetime is defined
            if (request.lifetime > 0f && spawned != null)
            {
                ScheduleDespawn(request.spawnID, spawned.gameObject, request.lifetime).Forget();
            }
        }


        /// <summary>
        /// Waits for the defined lifetime, then despawns the object.
        /// </summary>
        /// <param name="key">The addressables key used to return the object.</param>
        /// <param name="obj">The spawned GameObject instance.</param>
        /// <param name="lifetime">Time in seconds before despawning.</param>
        private async UniTask ScheduleDespawn(string key, GameObject obj, float lifetime)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(lifetime));
            if (obj != null)
            {
                ObjectManager.Instance.Return(key, obj.GetComponent<Component>());
            }
        }
    }
}
