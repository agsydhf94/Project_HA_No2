
namespace HA
{
    /// <summary>
    /// Event message published when the player's level progress changes.
    /// Contains both XP and level information.
    /// </summary>
    public class LevelProgressChangedEvent : IEventMessage
    {
        /// <summary>
        /// The player's current level.
        /// </summary>
        public int currentLevel;

        /// <summary>
        /// The player's current XP.
        /// </summary>
        public int currentXp;

        /// <summary>
        /// The required XP to reach the next level.
        /// </summary>
        public int requiredXp;

        /// <summary>
        /// The progress ratio (0–1) towards the next level.
        /// </summary>
        public float progress;

        public LevelProgressChangedEvent(int currentLevel, int currentXp, int requiredXp, float progress)
        {
            this.currentLevel = currentLevel;
            this.currentXp = currentXp;
            this.requiredXp = requiredXp;
            this.progress = progress;
        }
    }
}
