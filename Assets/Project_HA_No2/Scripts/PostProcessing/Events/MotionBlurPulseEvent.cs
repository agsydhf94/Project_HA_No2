
namespace HA
{
    /// <summary>
    /// Event message that requests a motion blur pulse.
    /// Carries an optional profile to override the default settings.
    /// </summary>
    public struct MotionBlurPulseEvent : IEventMessage
    {
        /// <summary>
        /// Motion blur pulse profile to use for this event.
        /// If null, the listener's default profile should be used.
        /// </summary>
        public MotionBlurPulseSO profile;

        /// <summary>
        /// Creates a new motion blur pulse event.
        /// </summary>
        /// <param name="profile">Optional profile that defines the pulse behavior.</param>
        public MotionBlurPulseEvent(MotionBlurPulseSO profile = null)
        {
            this.profile = profile;
        }
    }
}
