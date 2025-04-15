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

            // inventory 클래스에 있는 AddItem 부분만으로는 장비를 바꿔치기할 때만
            // 다시 원래의 인벤토리로 가져올 수 있는 상황이었지만
            // 이 부분 덕분에 현재 장착중인 장비에서 바로 탈착하여 바꿔치기할 필요 없이 인벤토리로 복귀 가능
            inventory.AddItem(item.itemDataSO as EquipmentDataSO);

            CleanUpSlot();
        }
    }
}
