using UnityEngine;
using DG.Tweening;

namespace HA
{
    /// <summary>
    /// Applies directional camera shake based on a configurable <see cref="ShakeProfileSO"/>.
    /// Automatically subscribes to <see cref="CameraShakeEvent"/> via the EventBus.
    /// Restores original camera position when shake ends.
    /// </summary>
    public class ProfiledCameraShaker : MonoBehaviour
    {
        /// <summary>
        /// Camera transform to apply the shake to. Defaults to main camera if not assigned.
        /// </summary>
        [SerializeField] private Transform targetCamera;

        /// <summary>
        /// Default shake profile to use when none is provided in the event.
        /// </summary>
        [SerializeField] private ShakeProfileSO defaultProfile;

        /// <summary>
        /// Cached local position of the camera before shake starts.
        /// </summary>
        private Vector3 originalLocalPos;

        /// <summary>
        /// Currently active shake tween, if any.
        /// </summary>
        private Tween shakeTween;

        private void Awake()
        {
            if (targetCamera == null)
                targetCamera = Camera.main.transform;

            originalLocalPos = targetCamera.localPosition;
        }

        private void OnEnable()
        {
            EventBus.Instance.Subscribe<CameraShakeEvent>(OnCameraShakeRequested);
        }

        private void OnDisable()
        {
            EventBus.Instance.Unsubscribe<CameraShakeEvent>(OnCameraShakeRequested);
        }


        /// <summary>
        /// Called when a camera shake is requested through the event bus.
        /// </summary>
        /// <param name="evt">The camera shake event payload, which may include a profile.</param>
        private void OnCameraShakeRequested(CameraShakeEvent evt)
        {
            PlayShake(evt.profile);
        }


        /// <summary>
        /// Starts a new camera shake based on the given or default profile.
        /// </summary>
        /// <param name="profile">Optional shake profile to override the default.</param>
        public void PlayShake(ShakeProfileSO profile = null)
        {
            profile ??= defaultProfile;

            if (shakeTween != null && shakeTween.IsActive())
                shakeTween.Kill();

            shakeTween = DOTween.To(() => 0f, t => ApplyShake(t, profile), 1f, profile.duration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    targetCamera.localPosition = originalLocalPos;
                });
        }


        /// <summary>
        /// Applies the per-frame camera offset based on the profile's intensity curve and direction.
        /// </summary>
        /// <param name="t">Normalized time (0 to 1).</param>
        /// <param name="profile">The shake profile to apply.</param>
        private void ApplyShake(float t, ShakeProfileSO profile)
        {
            float curveValue = profile.intensityCurve.Evaluate(t);
            float shakeNoise = Mathf.Sin(Time.time * profile.frequency * Mathf.PI * 2f); // 주기적인 진동

            Vector3 offset = profile.direction.normalized * curveValue * profile.strength * shakeNoise;
            targetCamera.localPosition = originalLocalPos + offset;
        }
    }
}
