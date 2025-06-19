using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    /// <summary>
    /// Utility class for reusable UI animations such as tab switching and fly-to-slot effects.
    /// Uses DOTween for smooth transition effects.
    /// </summary>
    public static class UIAnimationUtility
    {
        /// <summary>
        /// Smoothly transitions from one UI tab to another with fade and scale animation.
        /// </summary>
        /// <param name="fromTab">The currently active tab to fade out.</param>
        /// <param name="toTab">The tab to fade in and scale up.</param>
        /// <param name="duration">Duration of the transition in seconds.</param>
        public static void QuickTabSwitch(GameObject fromTab, GameObject toTab, float duration = 0.2f)
        {
            if (fromTab != null && fromTab.activeSelf)
            {
                CanvasGroup fromCG = GetOrAddCanvasGroup(fromTab);
                fromCG.DOFade(0f, duration).OnComplete(() => fromTab.SetActive(false));
            }

            if (toTab != null)
            {
                toTab.SetActive(true);
                CanvasGroup toCG = GetOrAddCanvasGroup(toTab);
                toCG.alpha = 0f;
                toTab.transform.localScale = Vector3.one * 0.95f;

                Sequence seq = DOTween.Sequence();
                seq.Join(toCG.DOFade(1f, duration));
                seq.Join(toTab.transform.DOScale(1f, duration).SetEase(Ease.OutSine));
            }
        }

        /// <summary>
        /// Adds a CanvasGroup to the specified GameObject if one does not already exist.
        /// </summary>
        /// <param name="go">Target GameObject.</param>
        /// <returns>The existing or newly added CanvasGroup.</returns>
        private static CanvasGroup GetOrAddCanvasGroup(GameObject go)
        {
            CanvasGroup cg = go.GetComponent<CanvasGroup>();
            if (cg == null) cg = go.AddComponent<CanvasGroup>();
            return cg;
        }


        /// <summary>
        /// Animates a UI icon flying from one RectTransform (e.g., inventory) to another (e.g., equipment slot).
        /// </summary>
        /// <param name="from">Starting position RectTransform.</param>
        /// <param name="to">Destination position RectTransform.</param>
        /// <param name="iconSprite">The sprite to animate.</param>
        /// <param name="canvasRoot">The parent transform, usually the root canvas.</param>
        /// <param name="duration">Duration of the animation in seconds.</param>
        /// <param name="scale">Scale factor for the icon size.</param>
        /// <param name="onComplete">Callback to invoke after the animation completes.</param>
        public static void AnimateItemFlyToSlot(
            RectTransform from,
            RectTransform to,
            Sprite iconSprite,
            Transform canvasRoot,
            float duration = 0.4f,
            float scale = 1.0f,
            System.Action onComplete = null
        )
        {
            if (from == null || to == null || iconSprite == null) return;

            var flyIcon = new GameObject("FlyIcon", typeof(Image));
            flyIcon.transform.SetParent(canvasRoot, false);

            var image = flyIcon.GetComponent<Image>();
            image.sprite = iconSprite;
            image.raycastTarget = false;
            image.preserveAspect = true;

            var rt = flyIcon.GetComponent<RectTransform>();
            rt.position = from.position;

            var targetSize = to.rect.size;
            rt.sizeDelta = targetSize * scale;

            rt.DOMove(to.position, duration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    GameObject.Destroy(flyIcon);
                    onComplete?.Invoke(); // Callback after animation completes
                });
        }
    }
}
