using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace HA
{
    /// <summary>
    /// Handles animated delayed health bar visuals using two UI Image components.
    /// One image updates immediately, while the other animates with delay to reflect gradual change.
    /// Useful for showing recent damage taken.
    /// </summary>
    public class DelayedFillController : MonoBehaviour
    {
        /// <summary>
        /// Image representing the immediate fill value (e.g., current value).
        /// </summary>
        [SerializeField] private Image immediateImage;

        /// <summary>
        /// Image representing the delayed fill value that animates toward the immediate value.
        /// </summary>
        [SerializeField] private Image delayedImage;

        /// <summary>
        /// Time to wait before the delayed image starts animating.
        /// </summary>
        [SerializeField] private float delay = 0.25f;

        /// <summary>
        /// Duration of the delayed image's shrink animation.
        /// </summary>
        [SerializeField] private float shrinkDuration = 0.6f;

        /// <summary>
        /// Color used for the delayed image during animation.
        /// </summary>
        [SerializeField] private Color delayedColor = Color.red;

        private float currentFill = 1f;
        private Tween shrinkTween;
        private bool hasDelayedStarted = false;


        /// <summary>
        /// Updates the fill amount of both immediate and delayed images.
        /// The delayed image animates after a short delay if it's the first update,
        /// or animates immediately if it's already started.
        /// </summary>
        /// <param name="normalized">The normalized fill amount (0 to 1).</param>
        public void SetFillAmount(float normalized)
        {
            currentFill = normalized;
            immediateImage.fillAmount = normalized;

            delayedImage.color = delayedColor;

            if (!hasDelayedStarted)
            {
                // Start the delayed animation with initial delay (only once)
                hasDelayedStarted = true;

                shrinkTween?.Kill();
                shrinkTween = delayedImage.DOFillAmount(normalized, shrinkDuration)
                    .SetDelay(delay)
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        shrinkTween = null;
                    });
            }
            else
            {
                // If already started, animate immediately (only if the value decreased)
                if (delayedImage.fillAmount > normalized)
                {
                    shrinkTween?.Kill();
                    shrinkTween = delayedImage.DOFillAmount(normalized, shrinkDuration)
                        .SetEase(Ease.OutCubic)
                        .OnComplete(() =>
                        {
                            shrinkTween = null;
                        });
                }
            }
        }

        /// <summary>
        /// Immediately synchronizes both images to the specified fill amount.
        /// Cancels any ongoing animation and resets delay logic.
        /// </summary>
        /// <param name="normalized">The normalized fill amount (0 to 1).</param>
        public void ForceSync(float normalized)
        {
            shrinkTween?.Kill();
            hasDelayedStarted = false;

            immediateImage.fillAmount = delayedImage.fillAmount = normalized;
        }
    }
}
