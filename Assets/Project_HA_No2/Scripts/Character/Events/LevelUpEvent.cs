
namespace HA
{
    /// <summary>
    /// Event message published when the player levels up.
    /// </summary>
    public struct LevelUpEvent : IEventMessage
    {
        /// <summary>
        /// The new level reached by the player.
        /// </summary>
        public int newLevel;
        public LevelUpEvent(int newLevel)
        {
            this.newLevel = newLevel;
        }
    }
}
