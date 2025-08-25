using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

namespace HA
{
    /// <summary>
    /// Component that applies and animates lens distortion pulses using DOTween and URP.
    /// Can listen for <see cref="LensDistortionPulseEvent"/> via EventBus or be triggered manually.
    /// </summary>
    public class LensDistortionPulse : MonoBehaviour
    {
        /// <summary>
        /// Default profile used for the distortion pulse.
        /// </summary>
        [SerializeField] private LensDistortionPulseSO profile;

        /// <summary>
        /// Whether to automatically subscribe to <see cref="LensDistortionPulseEvent"/>.
        /// </summary>
        [SerializeField] private bool listenEvent = true;

        /// <summary>
        /// The dynamically created volume controlling the lens distortion weight.
        /// </summary>
        private Volume volume;

        /// <summary>
        /// The LensDistortion effect instance added to the volume profile.
        /// </summary>                 
        private LensDistortion lensDistortion;

        /// <summary>
        /// The DOTween sequence used to animate the weight pulse.
        /// </summary>
        private Sequence sequence;


        /// <summary>
        /// Initializes the volume and lens distortion effect.
        /// </summary>
        void Awake()
        {
            volume = new GameObject("FX_LensDistortionVolume").AddComponent<Volume>();
            volume.transform.SetParent(transform, false);
            volume.isGlobal = true;
            volume.priority = 50f;
            volume.weight = 0f;
            volume.profile = ScriptableObject.CreateInstance<VolumeProfile>();

            lensDistortion = volume.profile.Add<LensDistortion>(true);
            lensDistortion.active = true;

            ApplyProfileToLens(profile);
        }


        /// <summary>
        /// Subscribes to the distortion event if enabled.
        /// </summary>
        void OnEnable()
        {
            if (listenEvent)
            {
                EventBus.Instance.Subscribe<LensDistortionPulseEvent>(OnEvent);
            }
                
        }


        /// <summary>
        /// Unsubscribes from the distortion event and resets the effect.
        /// </summary>
        void OnDisable()
        {
            if (listenEvent)
            {
                EventBus.Instance.Unsubscribe<LensDistortionPulseEvent>(OnEvent); KillAndReset();
            }
        }


        /// <summary>
        /// Called when a lens distortion event is received.
        /// </summary>
        void OnEvent(LensDistortionPulseEvent @event)
        {
            Play(@event.profile ?? profile);
        }


        /// <summary>
        /// Manually triggers the distortion pulse with the default profile.
        /// </summary>
        public void Trigger()
        {
            Play(profile);
        }


        /// <summary>
        /// Plays the distortion pulse with the specified profile.
        /// </summary>
        /// <param name="pulseSO">The profile defining the pulse parameters.</param>
        public void Play(LensDistortionPulseSO pulseSO)
        {
            if (pulseSO == null) return;
            ApplyProfileToLens(pulseSO);

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
        /// Applies a distortion profile's settings to the LensDistortion effect.
        /// </summary>
        /// <param name="pulseSO">The profile containing lens distortion parameters.</param>
        void ApplyProfileToLens(LensDistortionPulseSO pulseSO)
        {
            if (lensDistortion == null || pulseSO == null) return;

            lensDistortion.intensity.Override(pulseSO.peakIntensity);
            lensDistortion.xMultiplier.Override(pulseSO.xMultiplier);
            lensDistortion.yMultiplier.Override(pulseSO.yMultiplier);
            lensDistortion.center.Override(new Vector2(pulseSO.centerX, pulseSO.centerY));
            lensDistortion.scale.Override(pulseSO.scale);
        }


        /// <summary>
        /// Kills any active tween and resets the volume weight to zero.
        /// </summary>
        void KillAndReset()
        {
            sequence?.Kill();
            if (volume)
            {
                volume.weight = 0f;
            }
        }
    }
}
