using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    /// <summary>
    /// Controls the UI display for a single quest reward,
    /// showing its name, quantity, and icon.
    /// </summary>
    public class QuestRewardPrefab : MonoBehaviour
    {
        /// <summary>
        /// Text element displaying the name of the reward item.
        /// </summary>
        [SerializeField] private TMP_Text rewardNameText;

        /// <summary>
        /// Text element displaying the quantity of the reward item.
        /// </summary>
        [SerializeField] private TMP_Text rewardQuantityText;

        /// <summary>
        /// Image element displaying the icon of the reward item.
        /// </summary>
        [SerializeField] private Image rewardImage;


        /// <summary>
        /// Initializes the reward UI with item data and quantity.
        /// Displays the item name, icon (preserving aspect ratio), 
        /// and quantity if greater than or equal to 2.
        /// </summary>
        /// <param name="itemDataSO">The ScriptableObject containing the item's data.</param>
        /// <param name="quantity">The quantity of the reward item.</param>
        public void Initialize(ItemDataSO itemDataSO, int quantity)
        {
            rewardNameText.text = itemDataSO.itemName;
            rewardImage.sprite = itemDataSO.icon;
            rewardImage.preserveAspect = true;

            if (quantity >= 2)
            {
                rewardQuantityText.gameObject.SetActive(true);
                rewardQuantityText.text = quantity.ToString();
            }
        }
    }
}
