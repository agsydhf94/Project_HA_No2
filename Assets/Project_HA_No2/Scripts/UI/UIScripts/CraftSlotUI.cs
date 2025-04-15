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
            // 인벤토리 craft 아이템 정보
            EquipmentDataSO craftData = item.itemDataSO as EquipmentDataSO;

            if(inventory.CanCraft(craftData, craftData.requirementsForCraft))
            {
                // 장비 생성됨
                Debug.Log("장비 생성됨");
            }
        }
    }
}
