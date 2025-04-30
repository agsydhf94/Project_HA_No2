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

            canvasUI.itemToolTipUI.HideToolTip();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (item == null)
                return;

            Vector2 mousePosition = Input.mousePosition;

            float xOffset = 0f;
            float yOffset = 0f;

            if (mousePosition.x > 600f)
                xOffset = -100f;
            else
                xOffset = 100f;

            if (mousePosition.y > 600f)
                yOffset = -60f;
            else
                yOffset = 60f;

            canvasUI.itemToolTipUI.ShowToolTip(item.itemDataSO as EquipmentDataSO);
            canvasUI.itemToolTipUI.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (item == null)
                return;

            canvasUI.itemToolTipUI.HideToolTip();
        }
    }
}
