
namespace HA
{
    /// <summary>
    /// Event message published when the player gains experience points.
    /// </summary>
    public struct ExperienceGainedEvent : IEventMessage
    {
        /// <summary>
        /// Amount of experience gained.
        /// </summary>
        public int amount;
        public ExperienceGainedEvent(int amount)
        {
            this.amount = amount;
        }
    }
}
