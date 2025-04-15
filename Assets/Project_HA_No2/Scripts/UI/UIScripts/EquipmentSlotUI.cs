using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HA
{
    public class EquipmentSlotUI : ItemSlotUI
    {
        public EquipmentType equipmentSlotType;

        private void OnValidate()
        {
            gameObject.name = "Equipment Slot - " + equipmentSlotType.ToString();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            inventory.UnEquipEquipment(item.itemDataSO as EquipmentDataSO);

            // inventory Ŭ������ �ִ� AddItem �κи����δ� ��� �ٲ�ġ���� ����
            // �ٽ� ������ �κ��丮�� ������ �� �ִ� ��Ȳ�̾�����
            // �� �κ� ���п� ���� �������� ��񿡼� �ٷ� Ż���Ͽ� �ٲ�ġ���� �ʿ� ���� �κ��丮�� ���� ����
            inventory.AddItem(item.itemDataSO as EquipmentDataSO);

            CleanUpSlot();
        }
    }
}
