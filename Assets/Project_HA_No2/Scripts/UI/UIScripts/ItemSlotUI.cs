using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace HA
{
    public class ItemSlotUI : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemText;

        public InventoryItem item;

        public void UpdateSlot(InventoryItem newItem)
        {
            item = newItem;
            itemImage.preserveAspect = true;
            itemImage.color = Color.white;

            if (item != null)
            {
                itemImage.sprite = item.itemDataSO.icon;

                if (item.stackSize > 1)
                {
                    itemText.text = item.stackSize.ToString();
                }
                else
                {
                    itemText.text = "";
                }
            }
        }
    }
}
