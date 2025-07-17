using UnityEngine;
using DG.Tweening;

namespace HA
{
    /// <summary>
    /// Handles a combined fade and scale animation for UI elements using DOTween.
    /// This component animates the CanvasGroup's alpha and the RectTransform's scale 
    /// to smoothly show and hide the UI object.
    /// 
    /// Attach this script to a UI GameObject that has both CanvasGroup and RectTransform components.
    /// Call <see cref="PlayShow"/> to display the UI and <see cref="PlayHide"/> to hide it.
    /// </summary>
    public class UIFadeScaler : MonoBehaviour
    {
        [Header("Tween Settings")]
        public float fadeInDuration = 0.2f;
        public float fadeOutDuration = 0.15f;
        public float scaleInDuration = 0.25f;
        public float scaleOutDuration = 0.2f;
        public Ease easeIn = Ease.OutBack;
        public Ease easeOut = Ease.InBack;

        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private Tween fadeTween;
        private Tween scaleTween;

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
            canvasGroup.alpha = 0f;
            rectTransform.localScale = Vector3.zero;
        }


        /// <summary>
        /// Plays the show animation by fading in and scaling up the UI.
        /// </summary>
        public void PlayShow()
        {
            fadeTween?.Kill();
            scaleTween?.Kill();

            canvasGroup.alpha = 0f;
            rectTransform.localScale = Vector3.zero;

            fadeTween = canvasGroup.DOFade(1f, fadeInDuration);
            scaleTween = rectTransform.DOScale(1f, scaleInDuration).SetEase(easeIn);
        }


        /// <summary>
        /// Plays the hide animation by fading out and scaling down the UI.
        /// </summary>
        public void PlayHide()
        {
            fadeTween?.Kill();
            scaleTween?.Kill();

            fadeTween = canvasGroup.DOFade(0f, fadeOutDuration);
            scaleTween = rectTransform.DOScale(0f, scaleOutDuration).SetEase(easeOut);
        }
    }
}
