
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Static container holding commonly used game event message types.
    /// These are used to trigger specific game-related actions via the EventBus.
    /// </summary>
    public static class GameEvent
    {
        /// <summary>
        /// Event message broadcast when an enemy is defeated.
        /// </summary>
        public struct EnemyKilled : IEventMessage
        {
            /// <summary>
            /// The unique ID of the enemy that was killed.
            /// </summary>
            public string enemyID;
            /// <summary>
            /// Initializes a new EnemyKilled event with the given enemy ID.
            /// </summary>
            public EnemyKilled(string id) => enemyID = id;
        }

        /// <summary>
        /// Event message broadcast when an item is collected.
        /// </summary>
        public struct ItemCollected : IEventMessage
        {
            /// <summary>
            /// The unique ID of the item that was collected.
            /// </summary>
            public string itemID;

            /// <summary>
            /// Initializes a new ItemCollected event with the given item ID.
            /// </summary>
            public ItemCollected(string id) => itemID = id;
        }

        /// <summary>
        /// Generic spawn request supporting any spawnable type via string ID.
        /// </summary>
        public struct SpawnRequest : IEventMessage
        {
            public string spawnID;               // Addressable 키
            public Vector3 position;
            public Quaternion rotation;
            public float delay;                  // 스폰 전 딜레이
            public string vfxID;                 // VFX 키 (선택적)
            public float lifetime;               // 일정 시간 후 자동 파괴

            public SpawnRequest(
                string spawnID,
                Vector3 position,
                Quaternion rotation,
                float delay = 0f,
                string vfxID = null,
                float lifetime = 0f)
            {
                this.spawnID = spawnID;
                this.position = position;
                this.rotation = rotation;
                this.delay = delay;
                this.vfxID = vfxID;
                this.lifetime = lifetime;
            }
        }
    }
}
