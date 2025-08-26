using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

namespace HA
{
    /// <summary>
    /// Applies a timed motion blur pulse using a dynamically created URP Volume.
    /// Can auto-listen for <see cref="MotionBlurPulseEvent"/> via EventBus or be triggered manually.
    /// </summary>
    public class MotionBlurPulse : MonoBehaviour
    {
        /// <summary>
        /// Default pulse profile used when no override profile is provided.
        /// </summary>
        [SerializeField] private MotionBlurPulseSO profile;

        /// <summary>
        /// If true, subscribes to <see cref="MotionBlurPulseEvent"/> on enable and unsubscribes on disable.
        /// </summary>
        [SerializeField] private bool listenEvent = true;

        /// <summary>
        /// Dedicated URP Volume whose weight is tweened to play the pulse.
        /// </summary>
        private Volume volume;

        /// <summary>
        /// MotionBlur override added to the volume profile.
        /// </summary>               
        private MotionBlur motionBlur;

        /// <summary>
        /// DOTween sequence that animates the volume weight over time.
        /// </summary>
        private Sequence sequence;

        /// <summary>
        /// Creates a dedicated global Volume and initializes MotionBlur with the default profile.
        /// </summary>
        void Awake()
        {
            volume = new GameObject("FX_MotionBlurVolume").AddComponent<Volume>();
            volume.transform.SetParent(transform, false);
            volume.isGlobal = true;
            volume.priority = 50f;
            volume.weight = 0f;
            volume.profile = ScriptableObject.CreateInstance<VolumeProfile>();

            motionBlur = volume.profile.Add<MotionBlur>(true);
            motionBlur.active = true;

            ApplyProfileToBlur(profile);
        }


        /// <summary>
        /// Subscribes to event bus if listening is enabled.
        /// </summary>
        void OnEnable()
        {
            if (listenEvent)
            {
                EventBus.Instance.Subscribe<MotionBlurPulseEvent>(OnEvent);
            }
        }


        /// <summary>
        /// Unsubscribes from event bus (if enabled) and resets the volume weight.
        /// </summary>
        void OnDisable()
        {
            if (listenEvent)
            {
                EventBus.Instance.Unsubscribe<MotionBlurPulseEvent>(OnEvent); KillAndReset();
            }
        }


        /// <summary>
        /// Handles incoming motion blur pulse events.
        /// </summary>
        /// <param name="event">Event carrying an optional override profile.</param>
        void OnEvent(MotionBlurPulseEvent @event)
        {
            Play(@event.profile ?? profile);
        }


        /// <summary>
        /// Triggers a pulse using the default profile.
        /// </summary>
        public void Trigger()
        {
            Play(profile);
        }


        /// <summary>
        /// Plays a pulse using the specified profile (rise → hold → fall).
        /// </summary>
        /// <param name="pulseSO">Profile describing intensity and timing.</param>
        public void Play(MotionBlurPulseSO pulseSO)
        {
            if (pulseSO == null) return;
            ApplyProfileToBlur(pulseSO);

            sequence?.Kill();
            volume.weight = 0f;
            sequence = DOTween.Sequence()
                .Append(DOTween.To(() => volume.weight, w => volume.weight = w, 1f, pulseSO.upTime).SetEase(Ease.OutQuad))
                .AppendInterval(pulseSO.holdTime)
                .Append(DOTween.To(() => volume.weight, w => volume.weight = w, 0f, pulseSO.downTime).SetEase(Ease.OutCubic))
                .OnKill(() => volume.weight = 0f)
                .OnComplete(() => volume.weight = 0f);
        }


        /// <summary>
        /// Copies profile values into the MotionBlur override.
        /// </summary>
        /// <param name="pulseSO">Profile to read from.</param>
        void ApplyProfileToBlur(MotionBlurPulseSO pulseSO)
        {
            if (motionBlur == null || pulseSO == null) return;
            motionBlur.intensity.Override(pulseSO.peakIntensity);  
            motionBlur.clamp.Override(pulseSO.clamp);              
        }


        /// <summary>
        /// Stops any active tween and resets volume weight to zero.
        /// </summary>
        void KillAndReset()
        {
            sequence?.Kill();
            if (volume) volume.weight = 0f;
        }
    }
}
