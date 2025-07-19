
namespace HA
{
    /// <summary>
    /// Event message used to request a camera shake.
    /// Can be published through the EventBus to notify any listeners like <see cref="ProfiledCameraShaker"/>.
    /// </summary>
    public class CameraShakeEvent : IEventMessage
    {
        /// <summary>
        /// The shake profile to use. If null, the shaker will fall back to its default profile.
        /// </summary>
        public ShakeProfileSO profile;

        /// <summary>
        /// Constructs a new camera shake event with an optional profile.
        /// </summary>
        /// <param name="profile">The shake profile to use, or null to use the default.</param>
        public CameraShakeEvent(ShakeProfileSO profile = null)
        {
            this.profile = profile;
        }
    }
}
