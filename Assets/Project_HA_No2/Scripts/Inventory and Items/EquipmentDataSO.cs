using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public enum EquipmentType
    {
        Weapon,
        Armor,
        Amulet,
        Flask
    }

    [CreateAssetMenu(fileName = "EquipmentData", menuName = "DataSO/Equipment")]
    public class EquipmentDataSO : ItemDataSO
    {
        public EquipmentType equipmentType;
    }
}
