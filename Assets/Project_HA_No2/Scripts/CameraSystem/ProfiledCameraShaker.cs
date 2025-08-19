using UnityEngine;
using DG.Tweening;
using Cinemachine;

namespace HA
{
    /// <summary>
    /// Applies directional camera shake based on a configurable <see cref="ShakeProfileSO"/>.
    /// Subscribes to <see cref="CameraShakeEvent"/> via the EventBus and resets noise after shake ends.
    /// </summary>
    public class ProfiledCameraShaker : MonoBehaviour
    {
        /// <summary>
        /// Reference to the Cinemachine virtual camera used for applying shakes.
        /// </summary>
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        /// <summary>
        /// Default shake profile used if none is specified in the event.
        /// </summary>
        [SerializeField] private ShakeProfileSO defaultProfile;


        /// <summary>
        /// Cached Perlin noise component from the virtual camera.
        /// </summary>
        private CinemachineBasicMultiChannelPerlin noise;

        /// <summary>
        /// Currently active shake tween, if any.
        /// </summary>
        private Tween shakeTween;


        /// <summary>
        /// Validates required camera components and initializes noise settings.
        /// </summary>
        private void Awake()
        {
            if (virtualCamera == null)
            {
                Debug.LogError("[Shaker] Virtual Camera is not assigned.");
                enabled = false;
                return;
            }

            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise == null)
            {
                Debug.LogError("[Shaker] No Perlin Noise component found on virtual camera.");
                enabled = false;
                return;
            }

            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }


        /// <summary>
        /// Subscribes to the camera shake event on enable.
        /// </summary>
        private void OnEnable()
        {
            EventBus.Instance.Subscribe<CameraShakeEvent>(OnCameraShakeRequested);
        }


        /// <summary>
        /// Unsubscribes from the camera shake event on disable.
        /// </summary>
        private void OnDisable()
        {
            EventBus.Instance.Unsubscribe<CameraShakeEvent>(OnCameraShakeRequested);
        }


        /// <summary>
        /// Called when a camera shake event is received from the EventBus.
        /// </summary>
        /// <param name="evt">Shake event containing an optional <see cref="ShakeProfileSO"/>.</param>
        private void OnCameraShakeRequested(CameraShakeEvent evt)
        {
            PlayShake(evt.profile);
        }


        /// <summary>
        /// Starts a new camera shake using the given profile, or the default if none provided.
        /// </summary>
        /// <param name="profile">Shake profile to apply (optional).</param>
        public void PlayShake(ShakeProfileSO profile = null)
        {
            profile ??= defaultProfile;

            if (shakeTween != null && shakeTween.IsActive())
                shakeTween.Kill();

            // Set base noise values
            noise.m_AmplitudeGain = profile.strength;
            noise.m_FrequencyGain = profile.frequency;

            // Gradually reduce amplitude over time based on curve
            shakeTween = DOTween.To(() => 1f, t =>
            {
                float curveValue = profile.intensityCurve.Evaluate(1f - t);
                noise.m_AmplitudeGain = profile.strength * curveValue;
            }, 0f, profile.duration).SetEase(Ease.Linear)
              .OnComplete(() =>
              {
                  noise.m_AmplitudeGain = 0f;
                  noise.m_FrequencyGain = 0f;
              });
        }
    }
}
