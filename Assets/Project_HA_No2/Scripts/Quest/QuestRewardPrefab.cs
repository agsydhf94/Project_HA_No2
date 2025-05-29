using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class QuestRewardPrefab : MonoBehaviour
    {
        private ItemDataSO itemDataSO;

        private TMP_Text rewardNameText;
        private TMP_Text rewardQuantityText;
        private TMP_Text rewardAmountText;
        private Image rewardImage;

        public void Initialize(ItemDataSO itemDataSO, int quantity)
        {
            rewardNameText.text = itemDataSO.itemName;
            rewardImage.sprite = itemDataSO.icon;

            if(quantity >= 2)
            {
                rewardQuantityText.gameObject.SetActive(true);
                rewardAmountText.text = quantity.ToString();
            }

            if(itemDataSO.itemType == ItemType.Money)
            {
                rewardAmountText.gameObject.SetActive(true);
                
                MoneySO moneySO = itemDataSO as MoneySO;
                rewardAmountText.text = moneySO.moneyAmount.ToString();
            }

            if(itemDataSO.itemType == ItemType.Exp)
            {
                rewardAmountText.gameObject.SetActive(true);

                ExperienceSO expSO = itemDataSO as ExperienceSO;
                rewardAmountText.text = expSO.expAmount.ToString();
            }

        }
    }
}
