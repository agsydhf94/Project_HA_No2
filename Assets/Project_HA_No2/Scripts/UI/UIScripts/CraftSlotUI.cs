using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HA
{
    public class CraftSlotUI : ItemSlotUI
    {
        protected override void Start()
        {
            base.Start();
        }

        public void SetUpCraftSlot(EquipmentDataSO data)
        {
            if (data == null)
                return;

            item.itemDataSO = data;

            itemImage.preserveAspect = true;
            itemImage.sprite = data.icon;
            itemText.text = data.itemName;

            if(itemText.text.Length > 12)
            {
                itemText.fontSize *= 0.7f;
            }
            else
            {
                itemText.fontSize = 24f;
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            canvasUI.craftWindowUI.SetUpCraftWindow(item.itemDataSO as EquipmentDataSO);
        }
    }
}
