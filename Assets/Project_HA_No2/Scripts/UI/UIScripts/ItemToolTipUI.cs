using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace HA
{
    public class ItemToolTipUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private TMP_Text itemTypeText;
        [SerializeField] private TMP_Text itemDescription;

        [SerializeField] private float defaultFontSize = 32f;

        public void ShowToolTip(EquipmentDataSO item)
        {
            if (item == null)
                return;

            itemNameText.text = item.itemName;
            itemTypeText.text = item.equipmentType.ToString();
            itemDescription.text = item.GetDescription();

            if (itemNameText.text.Length > 12)
            {
                itemNameText.fontSize *= 0.7f;
            }
            else
            {
                itemNameText.fontSize = defaultFontSize;
            }

            gameObject.SetActive(true);
        }

        public void HideToolTip()
        {
            itemNameText.fontSize = defaultFontSize;

            gameObject.SetActive(false);
        }
    }
}
