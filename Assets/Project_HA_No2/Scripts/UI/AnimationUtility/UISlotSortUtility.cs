using System.Collections.Generic;

namespace HA
{
    /// <summary>
    /// Utility class for managing equipment UI slot assignments and restoring previous positions.
    /// </summary>
    public static class UISlotSortUtility
    {
        /// <summary>
        /// Captures the current slot assignments of EquipmentDataSO-based items
        /// and clears the slots for reassignment.
        /// </summary>
        /// <param name="slots">An array of EquipmentSlotUI elements.</param>
        /// <returns>
        /// A dictionary mapping each EquipmentDataSO to its previous slot index.
        /// </returns>
        public static Dictionary<EquipmentDataSO, int> CapturePreviousSlotAssignments(EquipmentSlotUI[] slots)
        {
            Dictionary<EquipmentDataSO, int> map = new();

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item?.itemDataSO is EquipmentDataSO data)
                    map[data] = i;

                slots[i].CleanUpSlot();     // Clear the slot for reuse
            }

            return map;
        }

        /// <summary>
        /// Reassigns inventory items to equipment slots based on their previous slot mappings.
        /// If a previous slot is not available, assigns the item to the first available compatible slot.
        /// </summary>
        /// <param name="itemMap">A dictionary of EquipmentDataSO and their corresponding InventoryItems.</param>
        /// <param name="slots">An array of EquipmentSlotUI elements.</param>
        /// <param name="previousSlotMap">Previously captured slot assignment map.</param>
        public static void ReassignItemsToSlots(
            Dictionary<EquipmentDataSO, InventoryItem> itemMap,
            EquipmentSlotUI[] slots,
            Dictionary<EquipmentDataSO, int> previousSlotMap)
        {
            foreach (var kvp in itemMap)
            {
                var data = kvp.Key;
                var item = kvp.Value;

                bool assigned = false;

                // Try to restore to previous slot
                if (previousSlotMap.TryGetValue(data, out int index))
                {
                    slots[index].UpdateSlot(item);
                    assigned = true;
                }

                // If previous slot unavailable, find first compatible empty slot
                if (!assigned)
                {
                    for (int i = 0; i < slots.Length; i++)
                    {
                        if (!slots[i].isUsing &&
                            data.equipmentType == slots[i].equipmentSlotType)
                        {
                            slots[i].UpdateSlot(item);
                            break;
                        }
                    }
                }
            }
        }
    }
}
