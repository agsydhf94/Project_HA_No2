using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    /// <summary>
    /// UI component that represents a single reward item in a quest reward panel.
    /// Displays the item icon, name, and amount with optional visual effects.
    /// </summary>
    public class QuestRewardItemUI : MonoBehaviour
    {
        /// <summary>
        /// UI image to display the item icon.
        /// </summary>
        [SerializeField] private Image iconImage;

        /// <summary>
        /// Text to display the name of the item.
        /// </summary>
        [SerializeField] private TextMeshProUGUI itemNameText;

        /// <summary>
        /// Text to display the amount of the item.
        /// </summary>
        [SerializeField] private TextMeshProUGUI amountText;

        /// <summary>
        /// Optional text typing effect for the item name.
        /// </summary>
        [SerializeField] private TextTyper itemNameTextTyper;

        /// <summary>
        /// Optional text typing effect for the item amount.
        /// </summary>
        [SerializeField] private TextTyper amountTextTyper;

        /// <summary>
        /// Fade scaling animation for showing the item.
        /// </summary>
        public UIFadeScaler uIFadeScaler;

        /// <summary>
        /// Fly animation effect for the item.
        /// </summary>
        public FlyEffect flyEffect;

        /// <summary>
        /// Color to apply to the icon image.
        /// </summary>
        public Color imageColor;

        void Awake()
        {
            flyEffect = GetComponent<FlyEffect>();
        }


        /// <summary>
        /// Sets the item display with icon, name, and amount.
        /// </summary>
        /// <param name="item">The item data to display.</param>
        /// <param name="amount">The quantity of the item.</param>
        public void Set(ItemDataSO item, int amount)
        {
            iconImage.sprite = item.icon;
            iconImage.preserveAspect = true;
            iconImage.color = imageColor;

            itemNameText.text = item.itemName;

            if (amount > 1)
            {
                amountText.text = $"x {amount}";
            }
        }
    }
}
