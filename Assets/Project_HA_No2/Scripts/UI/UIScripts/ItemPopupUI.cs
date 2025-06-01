using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class ItemPopupUI : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemName;

        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;

        private float moveDistance = 200f;
        private float fadeDuration = 0.3f;
        private float popupDuration;

        private Tween currentTween;
        private System.Action<ItemPopupUI> onComplete;

        public void Initialize(ItemDataSO itemDataSO, float duration, System.Action<ItemPopupUI> onCompleteCallback)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();

            popupDuration = duration;
            onComplete = onCompleteCallback;

            itemImage.sprite = itemDataSO.icon;
            itemImage.preserveAspect = true;

            itemName.text = itemDataSO.name;

            // 초기 상태
            canvasGroup.alpha = 0f;
            rectTransform.anchoredPosition = new Vector2(-moveDistance, rectTransform.anchoredPosition.y);

            PlayAnimation();       
        }

        private void PlayAnimation()
        {
            Sequence sequence = DOTween.Sequence();

            // Fade in + Slide in
            sequence.Append(canvasGroup.DOFade(1f, fadeDuration));
            sequence.Join(rectTransform.DOAnchorPosX(0f, fadeDuration).SetEase(Ease.OutQuad));

            // Display duration
            sequence.AppendInterval(popupDuration);

            // Fade out + Slide out
            sequence.Append(canvasGroup.DOFade(0f, fadeDuration));
            sequence.Join(rectTransform.DOAnchorPosX(-moveDistance, fadeDuration).SetEase(Ease.InQuad));

            sequence.OnComplete(() => onComplete?.Invoke(this));
            currentTween = sequence;
        }

        public void SetVerticalOffset(float offset)
        {
            rectTransform.DOAnchorPosY(offset, 0.2f).SetEase(Ease.OutCubic);
        }

        private void OnDisable()
        {
            currentTween?.Kill();
        }
    }
}
