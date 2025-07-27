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


        /// <summary>
        /// Updates the fill values of both immediate and delayed images.
        /// The delayed image animates with delay to reflect change.
        /// </summary>
        /// <param name="normalized">Normalized fill value (0 to 1).</param>
        public void SetFillAmount(float normalized)
        {
            currentFill = normalized;
            immediateImage.fillAmount = normalized;

            delayedImage.color = delayedColor;
            delayedImage.DOKill();
            delayedImage.DOFillAmount(normalized, shrinkDuration)
                      .SetDelay(delay)
                      .SetEase(Ease.OutCubic);
        }


        /// <summary>
        /// Instantly sets both immediate and delayed images to the given fill amount,
        /// bypassing animation.
        /// </summary>
        /// <param name="normalized">Normalized health value (0 to 1).</param>
        public void ForceSync(float normalized)
        {
            immediateImage.fillAmount = delayedImage.fillAmount = normalized;
        }
    }
}
