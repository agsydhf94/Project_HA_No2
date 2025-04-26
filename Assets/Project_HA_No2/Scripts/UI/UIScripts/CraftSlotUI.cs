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

            itemImage.sprite = data.icon;
            itemText.text = data.itemName;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            // �κ��丮 craft ������ ����
            EquipmentDataSO craftData = item.itemDataSO as EquipmentDataSO;

            if(inventory.CanCraft(craftData, craftData.requirementsForCraft))
            {
                // ��� ������
                Debug.Log("��� ������");
            }
        }
    }
}
