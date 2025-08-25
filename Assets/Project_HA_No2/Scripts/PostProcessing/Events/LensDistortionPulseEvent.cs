
namespace HA
{
    /// <summary>
    /// Event message used to trigger a lens distortion pulse effect.
    /// </summary>
    public class LensDistortionPulseEvent : IEventMessage
    {
        /// <summary>
        /// The distortion profile associated with this event.
        /// If null, the default profile will be used.
        /// </summary>
        public LensDistortionPulseSO profile;


        /// <summary>
        /// Creates a new lens distortion pulse event.
        /// </summary>
        /// <param name="profile">The profile to use for the pulse (optional).</param>
        public LensDistortionPulseEvent(LensDistortionPulseSO profile = null)
        {
            this.profile = profile;
        }
    }
}
