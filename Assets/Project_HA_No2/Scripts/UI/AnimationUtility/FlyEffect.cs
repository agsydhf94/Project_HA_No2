using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;

namespace HA
{
    /// <summary>
    /// Defines whether the fly effect is an entry (FlyIn) or an exit (FlyOut).
    /// </summary>
    public enum FlyMode
    {
        FlyIn,
        FlyOut
    }

    /// <summary>
    /// Plays a directional fly-in or fly-out animation by offsetting the RectTransform 
    /// from or to a specified position using DOTween. Intended for UI elements such as reward icons.
    /// </summary>
    public class FlyEffect : MonoBehaviour
    {
        [Header("Fly Settings")]

        /// <summary>
        /// The animation mode: FlyIn (enter) or FlyOut (exit).
        /// </summary>
        public FlyMode mode = FlyMode.FlyIn;

        /// <summary>
        /// Offset vector used to determine the fly direction and distance. 
        /// For example, (0, -300) means flying in from below.
        /// </summary>
        public Vector2 offset = new Vector2(0, -300f);

        /// <summary>
        /// Duration of the fly animation in seconds.
        /// </summary>
        public float duration = 0.5f;

        /// <summary>
        /// Easing function to control animation curve.
        /// </summary>
        public Ease ease = Ease.OutBack;
        

        [Header("Execution")]

        /// <summary>
        /// If true, the effect will automatically play when the object is enabled.
        /// Set to false to trigger manually via code (lazy mode).
        /// </summary>
        public bool playOnEnable = false; // Lazy 모드 컨트롤

        private Vector3 originalPosition;
        private RectTransform rect;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            if (playOnEnable)
            {
                Play(); // 자동 재생
            }
        }

        /// <summary>
        /// Immediately plays the fly effect.
        /// The element is offset and then animated back (FlyIn),
        /// or animated to the offset position (FlyOut).
        /// </summary>
        public void Play()
        {
            if (rect == null)
                rect = GetComponent<RectTransform>();

            originalPosition = rect.localPosition;

            if (mode == FlyMode.FlyIn)
            {
                rect.localPosition = originalPosition + (Vector3)offset;
                rect.DOLocalMove(originalPosition, duration).SetEase(ease);
            }
            else if (mode == FlyMode.FlyOut)
            {
                rect.DOLocalMove(originalPosition + (Vector3)offset, duration).SetEase(ease);
            }
        }

        /// <summary>
        /// Plays the fly effect after a specified delay.
        /// Used to stagger animations when displaying multiple UI elements.
        /// </summary>
        /// <param name="delay">Time in seconds to wait before playing the animation.</param>
        public async UniTask PlayWithDelayAsync(float delay)
        {
            if (rect == null)
                rect = GetComponent<RectTransform>();

            // Cache the original layout position
            originalPosition = rect.localPosition;

            // Move instantly to the offset position
            rect.localPosition = originalPosition + (Vector3)offset;

            // Wait before animating back to original position
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            rect.DOLocalMove(originalPosition, duration).SetEase(ease);
        }
    }
}
