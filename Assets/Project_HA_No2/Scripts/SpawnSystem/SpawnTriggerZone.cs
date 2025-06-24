using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Triggers the spawning of predefined entries when a player enters the zone.
    /// Publishes spawn requests via the EventBus and supports optional one-time activation.
    /// </summary>
    public class SpawnTriggerZone : MonoBehaviour
    {
        /// <summary>List of spawnable entries triggered by this zone.</summary>
        [SerializeField] private SpawnableEntry[] spawnEntries;

        /// <summary>If true, the trigger will activate only once.</summary>
        [SerializeField] private bool oneTimeTrigger = true;

        private bool triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (triggered && oneTimeTrigger) return;
            if (!other.CompareTag("Player")) return;

            triggered = true;

            foreach (var entry in spawnEntries)
            {
                var request = new GameEvent.SpawnRequest
                (
                    spawnID: entry.spawnID,
                    position: transform.position + entry.offset,
                    rotation: entry.rotation,
                    delay: entry.delay,
                    lifetime: entry.lifetime,
                    vfxID: entry.vfxID
                );

                EventBus.Instance.Publish(request);
            }
        }
    }
}
