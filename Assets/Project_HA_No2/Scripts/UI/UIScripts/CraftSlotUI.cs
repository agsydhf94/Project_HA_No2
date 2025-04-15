using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HA
{
    public class CraftSlotUI : ItemSlotUI
    {
        private void OnEnable()
        {
            UpdateSlot(item);
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
