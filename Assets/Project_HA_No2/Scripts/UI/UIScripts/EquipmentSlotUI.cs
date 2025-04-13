using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EquipmentSlotUI : ItemSlotUI
    {
        public EquipmentType equipmentSlotType;

        private void OnValidate()
        {
            gameObject.name = "Equipment Slot - " + equipmentSlotType.ToString();
        }
    }
}
