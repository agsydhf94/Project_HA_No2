using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class Inventory : SingletonBase<Inventory>
    {
        [Header("Inventory Items")]
        public List<InventoryItem> inventoryItems;
        public Dictionary<ItemDataSO, InventoryItem> inventoryDictionary;

        [Header("InventoryUI")]
        [SerializeField] private Transform inventorySlotParent;

        private ItemSlotUI[] itemSlots;

        private void Start()
        {
            inventoryItems = new List<InventoryItem>();
            inventoryDictionary = new Dictionary<ItemDataSO, InventoryItem>();

            itemSlots = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
        }

        // 아이템 픽업이나 추가할 때 이 메서드를 호출
        private void UpdateSlotUI()
        {
            for(int i = 0; i < inventoryItems.Count; i++)
            {
                itemSlots[i].UpdateSlot(inventoryItems[i]);
            }
        }

        public void AddItem(ItemDataSO item)
        {
            if(inventoryDictionary.TryGetValue(item, out InventoryItem value))
            {
                value.AddStack();
            }
            else
            {
                InventoryItem newItem = new InventoryItem(item);
                inventoryItems.Add(newItem);
                inventoryDictionary.Add(item, newItem);
            }

            UpdateSlotUI();
        }

        public void RemoveItem(ItemDataSO item)
        {
            if(inventoryDictionary.TryGetValue(item, out InventoryItem value))
            {
                if(value.stackSize <= 1)
                {
                    inventoryItems.Remove(value);
                    inventoryDictionary.Remove(item);
                }
                else
                {
                    value.RemoveStack();
                }
            }

            UpdateSlotUI();
        }
    }
}
