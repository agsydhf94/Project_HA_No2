using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

namespace HA
{
    public class ItemSlotUI : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemText;

        public InventoryItem item;
        private Inventory inventory;

        private void Awake()
        {
            inventory = Inventory.Instance;
        }

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

        public void CleanUpSlot()
        {
            item = null;

            itemImage.sprite = null;
            itemImage.color = Color.clear;

            itemText.text = "";
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(item.itemDataSO.itemType == ItemType.Equipment)
            {
                inventory.EquipEquipment(item.itemDataSO);
            }
        }
        

    }
}
