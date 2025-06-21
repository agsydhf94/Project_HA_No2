using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    /// <summary>
    /// Utility class that provides fly animation effects for item slot UI transitions.
    /// </summary>
    public static class UIFlyEffectUtility
    {
        /// <summary>
        /// Plays a fly-to-slot visual effect for equipping an item.
        /// Temporarily disables the source and target icons, animates the item icon flying from the source to the target,
        /// then re-enables the icons after the animation is complete.
        /// </summary>
        /// <param name="itemDataSO">The item data containing the icon to animate.</param>
        /// <param name="fromRect">The source RectTransform from which the icon will fly.</param>
        /// <param name="targetSlot">The UI slot to which the icon will fly.</param>
        /// <param name="onComplete">Optional callback to invoke after the animation completes.</param>
        public static void TryPlayEquipSlotFlyEffect(ItemDataSO itemDataSO, RectTransform fromRect, ItemSlotUI targetSlot, System.Action onComplete = null)
        {
            if (targetSlot == null || itemDataSO == null) return;

            var targetImage = targetSlot.itemImage;
            var fromImage = fromRect?.GetComponent<Image>();

            // Temporarily hide source and target images during animation
            if (fromImage != null)
                fromImage.enabled = false;
            targetImage.enabled = false;

            // Animate icon flying to target slot
            UIAnimationUtility.AnimateItemFlyToSlot(
                from: fromRect,
                to: targetImage.rectTransform,
                iconSprite: itemDataSO.icon,
                canvasRoot: CanvasUI.Instance.transform,
                onComplete: () =>
                {
                    onComplete?.Invoke();
                    
                    // Re-enable images after animation
                    targetImage.enabled = true;
                    if (fromImage != null)
                        fromImage.enabled = true;
                }
            );
        }
    }
}
