using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace HA
{
    public class PlayerItemDrop : ItemDrop
    {
        private Inventory inventory;

        [Header("Player Drops")]
        [SerializeField] private float changeOfLossingEquipments;
        [SerializeField] private float changeOfLossingMaterials;

        private void Start()
        {
            inventory = Inventory.Instance;
        }

        public override void GenerateDropItem()
        {

            List<InventoryItem> equipmentsToUnequip = new List<InventoryItem>();
            List<InventoryItem> materialsToLose = new List<InventoryItem>();

            foreach(InventoryItem item in inventory.GetEquipmentList())
            {
                if(Random.Range(0, 100) <= changeOfLossingEquipments)
                {
                    DropItem(item.itemDataSO);
                    equipmentsToUnequip.Add(item);       
                }
            }

            foreach (InventoryItem item in equipmentsToUnequip)
            {
                inventory.UnEquipEquipment(item.itemDataSO as EquipmentDataSO);
            }


            foreach(InventoryItem item in inventory.GetStashList())
            {
                if (Random.Range(0, 100) <= changeOfLossingEquipments)
                {
                    DropItem(item.itemDataSO);
                    materialsToLose.Add(item);
                }
            }

            foreach (InventoryItem item in materialsToLose)
            {
                inventory.RemoveItem(item.itemDataSO);
            }
        }
    }
}
