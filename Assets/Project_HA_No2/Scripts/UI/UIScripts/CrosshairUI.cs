using UnityEngine;
using DG.Tweening;

namespace HA
{
    public class CrosshairUI : MonoBehaviour
    {
        [Header("Elements")]
        public RectTransform top;
        public RectTransform bottom;
        public RectTransform left;
        public RectTransform right;

        [Header("Animation Settings")]
        public float idleSpread = 40f;
        public float aimSpread = 10f;
        public float transitionTime = 0.2f;
        public Ease ease = Ease.OutCubic;

        private bool isAiming = false;

        private void Start()
        {
            SetSpreadInstant(idleSpread);
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public void SetAiming(bool aiming)
        {
            if (isAiming == aiming) return;
            isAiming = aiming;

            float target = aiming ? aimSpread : idleSpread;
            AnimateToSpread(target);
        }

        private void AnimateToSpread(float spread)
        {
            top.DOAnchorPos(Vector2.up * spread, transitionTime).SetEase(ease);
            bottom.DOAnchorPos(Vector2.down * spread, transitionTime).SetEase(ease);
            left.DOAnchorPos(Vector2.left * spread, transitionTime).SetEase(ease);
            right.DOAnchorPos(Vector2.right * spread, transitionTime).SetEase(ease);
        }

        private void SetSpreadInstant(float spread)
        {
            top.anchoredPosition = Vector2.up * spread;
            bottom.anchoredPosition = Vector2.down * spread;
            left.anchoredPosition = Vector2.left * spread;
            right.anchoredPosition = Vector2.right * spread;
        }
    }
}
