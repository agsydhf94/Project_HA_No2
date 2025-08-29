using UnityEngine;

namespace HA
{
    /// <summary>
    /// Manages player-related data such as character, currency, and level/XP progression.
    /// Handles saving/loading and publishes events when XP or level changes.
    /// </summary>
    public class PlayerManager : SingletonBase<PlayerManager>, ISaveManager
    {
        public PlayerCharacter playerCharacter;

        /// <summary>
        /// Configuration ScriptableObject defining level progression rules.
        /// </summary>
        public LevelConfigSO levelConfig;

        /// <summary>
        /// Current in-game currency owned by the player.
        /// </summary>
        public int currency;

        [Header("Level / XP")]
        /// <summary>
        /// Current level of the player character.
        /// </summary>
        [Min(1)] public int currentCharacterLevel = 1;

        /// <summary>
        /// Current accumulated experience points.
        /// </summary>
        [Min(0)] public int currentXp = 0;


        /// <summary>
        /// Deducts the given price if enough money is available.
        /// Returns true if purchase succeeded, false otherwise.
        /// </summary>
        /// <param name="price">The amount of money required for the purchase.</param>
        /// <returns>True if the player had enough currency and the price was deducted, false otherwise.</returns>
        public bool CheckEnoughMoney(int price)
        {
            if (price > currency)
            {
                return false;
            }

            currency -= price;
            return true;
        }

        /// <summary>
        /// Returns the player's current money amount.
        /// </summary>
        public int GetCurrentMoney() => currency;


        /// <summary>
        /// Calculates required XP to advance from the specified level to the next.
        /// </summary>
        /// <param name="level">The current level to check against.</param>
        /// <returns>
        /// The required XP for progressing to the next level.
        /// if the level is invalid or exceeds the maximum.
        /// </returns>
        public int GetRequiredXp(int level)
        {
            if (levelConfig == null) return int.MaxValue;
            if (level >= levelConfig.maxLevel) return int.MaxValue;

            var list = levelConfig.requiredXp;
            if (list == null || list.Count <= level) return int.MaxValue; // 안전 가드

            return Mathf.Max(1, list[level]);
        }

        /// <summary>
        /// Gets the XP required to reach the next level from the current level.
        /// </summary>
        public int GetRequiredXpForNextLevel() => GetRequiredXp(currentCharacterLevel);


        /// <summary>
        /// Returns normalized XP progress (0–1) toward the next level.
        /// </summary>
        public float GetXpProgress()
        {
            if (levelConfig == null) return 0f;
            if (currentCharacterLevel >= levelConfig.maxLevel) return 1f;

            int req = GetRequiredXpForNextLevel();
            return req <= 0 ? 0f : Mathf.Clamp01((float)currentXp / req);
        }


        /// <summary>
        /// Adds experience points, handles level-ups, publishes relevant events,
        /// and plays level-up sound effects when configured.
        /// </summary>
        /// <param name="amount">The amount of XP to add. Values ≤ 0 are ignored.</param>
        public void AddExperience(int amount)
        {
            if (amount <= 0 || levelConfig == null) return;

            currentXp += amount;
            EventBus.Instance.Publish(new ExperienceGainedEvent(amount));
            PublishProgressChanged();

            while (currentCharacterLevel < levelConfig.maxLevel)
            {
                int req = GetRequiredXp(currentCharacterLevel);
                if (currentXp < req) break;

                currentXp -= req;
                currentCharacterLevel++;

                EventBus.Instance.Publish(new LevelUpEvent(currentCharacterLevel));
                TryPlayLevelUpSfx();
                PublishProgressChanged();
            }

            if (currentCharacterLevel >= levelConfig.maxLevel)
            {
                currentXp = 0;
                PublishProgressChanged();
            }
        }


        /// <summary>
        /// Publishes a <see cref="LevelProgressChangedEvent"/> with updated progress data.
        /// </summary>
        private void PublishProgressChanged()
        {
            int req = GetRequiredXpForNextLevel();
            EventBus.Instance.Publish(new LevelProgressChangedEvent(
                currentCharacterLevel,
                currentXp,
                req == int.MaxValue ? 0 : req,
                GetXpProgress()
            ));
        }


        /// <summary>
        /// Attempts to play the level-up SFX using the configured sound key.
        /// </summary>        
        private void TryPlayLevelUpSfx()
        {
            if (string.IsNullOrEmpty(levelConfig.levelUpSfxKey)) return;

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySound(levelConfig.levelUpSfxKey);
            }
        }

        /// <summary>
        /// Loads persistent player data (currency, level, XP) from a <see cref="GameData"/> instance.
        /// </summary>
        /// <param name="data">The <see cref="GameData"/> instance containing saved values.</param>
        public void LoadData(GameData data)
        {
            currency = data.currency;
            currentCharacterLevel = data.currentCharacterLevel;
            currentXp = data.currentXp;
        }


        /// <summary>
        /// Saves persistent player data (currency, level, XP) into a <see cref="GameData"/> instance.
        /// </summary>
        /// <param name="data">
        /// Reference to the <see cref="GameData"/> structure that will be updated with player state.
        /// </param>    
        public void SaveData(ref GameData data)
        {
            data.currency = currency;
            data.currentCharacterLevel = currentCharacterLevel;
            data.currentXp = currentXp;
        }
    }
}
