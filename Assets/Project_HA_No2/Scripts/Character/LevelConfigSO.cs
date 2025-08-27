using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// ScriptableObject that defines level progression configuration.
    /// Stores maximum level, required XP per level, growth multiplier,
    /// and optional key for level-up sound effects.
    /// </summary>
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "DataSO/LevelConfig")]
    public class LevelConfigSO : ScriptableObject
    {
        /// <summary>
        /// Maximum level the player can reach.
        /// </summary>
        [Min(1)] public int maxLevel;

        /// <summary>
        /// XP required for each level. Indexed starting at level 1.
        /// </summary>
        public List<int> requiredXp;

        /// <summary>
        /// Growth factor used for scaling XP requirements dynamically.
        /// </summary>
        public float growth;

        /// <summary>
        /// Key to play the level-up sound effect.
        /// </summary>
        public string levelUpSfxKey;
    }
}
