using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

namespace HA
{
    public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Image itemImage;
        [SerializeField] protected TMP_Text itemText;

        protected CanvasUI canvasUI;
        public InventoryItem item;
        protected Inventory inventory;

        protected virtual void Start()
        {
            inventory = Inventory.Instance;
            canvasUI = GetComponentInParent<CanvasUI>();
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

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (item == null)
                return;

            if(Input.GetKey(KeyCode.LeftControl))
            {
                inventory.RemoveItem(item.itemDataSO);
                return;
            }

            if(item.itemDataSO.itemType == ItemType.Equipment)
            {
                inventory.EquipEquipment(item.itemDataSO);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (item == null)
                return;

            canvasUI.itemToolTipUI.ShowToolTip(item.itemDataSO as EquipmentDataSO);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (item == null)
                return;

            canvasUI.itemToolTipUI.HideToolTip();
        }
    }
}
