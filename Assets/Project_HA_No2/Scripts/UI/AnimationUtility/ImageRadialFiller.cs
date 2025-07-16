using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace HA
{
    /// <summary>
    /// Controls a radial fill animation for an Image component using DOTween.
    /// Can be configured to auto-play or reset on enable.
    /// Supports callback on fill completion.
    /// </summary>
    public class ImageRadialFiller : MonoBehaviour
    {
        [Header("Fill Settings")]
        public float duration = 1f;
        public Ease ease = Ease.OutCubic;
        public bool autoPlayOnEnable = false;
        public bool resetOnEnable = true;

        private Image targetImage;
        private Tweener currentTween;

        /// <summary>
        /// Event invoked when the fill animation is completed.
        /// </summary>
        public Action onFillComplete;

        /// <summary>
        /// Initializes and auto-configures the Image to use Radial360 fill type if necessary.
        /// </summary>
        void Awake()
        {
            targetImage = GetComponent<Image>();
            if (targetImage.type != Image.Type.Filled)
            {
                Debug.LogWarning($"[RadialFill] Image '{name}' is not set to Filled. Auto-configuring.");
                targetImage.type = Image.Type.Filled;
                targetImage.fillMethod = Image.FillMethod.Radial360;
            }
        }


        /// <summary>
        /// Resets the fill amount and optionally plays the fill animation when enabled.
        /// </summary>
        void OnEnable()
        {
            if(resetOnEnable)
                targetImage.fillAmount = 0f;
            
            if (autoPlayOnEnable)
                Play();
        }


        /// <summary>
        /// Starts the radial fill animation from 0 to 1 over the specified duration.
        /// </summary>
        public void Play()
        {
            currentTween?.Kill();

            targetImage.fillAmount = 0f;
            currentTween = targetImage.DOFillAmount(1f, duration)
                .SetEase(ease)
                .OnComplete(() => onFillComplete?.Invoke());
        }


        /// <summary>
        /// Starts the fill animation from a specified starting value to 1.
        /// </summary>
        /// <param name="fromAmount">Starting fill amount (0 to 1).</param>
        public void PlayFrom(float fromAmount)
        {
            currentTween?.Kill();

            targetImage.fillAmount = fromAmount;
            currentTween = targetImage.DOFillAmount(1f, duration * (1f - fromAmount))
                .SetEase(ease)
                .OnComplete(() => onFillComplete?.Invoke());
        }


        /// <summary>
        /// Stops the current fill animation if it's playing.
        /// </summary>
        public void Stop()
        {
            currentTween?.Kill();
        }
    }
}

