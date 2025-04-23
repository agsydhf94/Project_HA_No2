using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class Inventory : SingletonBase<Inventory>
    {
        public List<ItemDataSO> initialEquipment;

        public List<InventoryItem> equipment;
        public Dictionary<EquipmentDataSO, InventoryItem> equipmentDictionary;

        [Header("Inventory Items")]
        public List<InventoryItem> inventory;
        public Dictionary<ItemDataSO, InventoryItem> inventoryDictionary;

        public List<InventoryItem> stash;
        public Dictionary<ItemDataSO, InventoryItem> stashDictionary;

        [Header("InventoryUI")]
        [SerializeField] private Transform inventorySlotParent;
        [SerializeField] private Transform stashSlotParent;
        [SerializeField] private Transform equipmentSlotParent;

        private ItemSlotUI[] inventoryItemSlots;
        private ItemSlotUI[] stashItemSlots;
        private EquipmentSlotUI[] equipmentSlots;

        [Header("Items information")]
        private float lastTimeUsedPotion;
        private float lastTimeUsedArmor;
        private float potionCooldown;
        private float armorCooldown;

        private void Start()
        {            
            inventory = new List<InventoryItem>();
            inventoryDictionary = new Dictionary<ItemDataSO, InventoryItem>();

            stash = new List<InventoryItem>();
            stashDictionary = new Dictionary<ItemDataSO, InventoryItem>();

            equipment = new List<InventoryItem>();
            equipmentDictionary = new Dictionary<EquipmentDataSO, InventoryItem>();

            inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
            stashItemSlots = stashSlotParent.GetComponentsInChildren<ItemSlotUI>();
            equipmentSlots = equipmentSlotParent.GetComponentsInChildren<EquipmentSlotUI>();

            InitializeItems();
        }

        private void Update()
        {
            
        }

        private void InitializeItems()
        {
            for (int i = 0; i < initialEquipment.Count; i++)
            {
                AddItem(initialEquipment[i]);
            }
        }

        public void EquipEquipment(ItemDataSO item)
        {
            EquipmentDataSO newEquipment = item as EquipmentDataSO;
            InventoryItem newItem = new InventoryItem(newEquipment);

            EquipmentDataSO oldEquipment = null;

            foreach (KeyValuePair<EquipmentDataSO, InventoryItem> _item in equipmentDictionary)
            {
                if (_item.Key.equipmentType == newEquipment.equipmentType)
                {
                    oldEquipment = _item.Key;
                }
            }

            if(oldEquipment != null)
            {
                // �̹� ������ ���(oldEquipment)�� ������ Equipment â���� ��������ϰ�
                UnEquipEquipment(oldEquipment);

                // �ش� ���� ���� ������ ���⿡ ���� �ٽ� �κ��丮�� ���ư��� �Ѵ�.
                AddItem(oldEquipment);
            }

            // �̹� ������ ���(oldEquipment)�� ���ٸ�
            // ���� ������ ��� �����ϴ� ��(equipment, equipmentDictionary)�� ������ �ְ�
            // �κ��丮���� �����
            equipment.Add(newItem);
            equipmentDictionary.Add(newEquipment, newItem);
            newEquipment.AddModifiers();

            RemoveItem(item);

            UpdateSlotUI();
        }

        public void UnEquipEquipment(EquipmentDataSO itemToRemove)
        {
            if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
            {
                equipment.Remove(value);
                equipmentDictionary.Remove(itemToRemove);
                itemToRemove.RemoveModifiers();
            }
        }

        // ������ �Ⱦ��̳� �߰��� �� �� �޼��带 ȣ��
        private void UpdateSlotUI()
        {
            for(int i = 0; i < equipmentSlots.Length; i++)
            {
                foreach (KeyValuePair<EquipmentDataSO, InventoryItem> _item in equipmentDictionary)
                {
                    if (_item.Key.equipmentType == equipmentSlots[i].equipmentSlotType)
                    {
                        equipmentSlots[i].UpdateSlot(_item.Value);
                    }
                }
            }

            for(int i = 0; i < inventoryItemSlots.Length; i++)
            {
                inventoryItemSlots[i].CleanUpSlot();
            }
            for(int i = 0; i < stashItemSlots.Length; i++)
            {
                stashItemSlots[i].CleanUpSlot();
            }


            for(int i = 0; i < inventory.Count; i++)
            {
                inventoryItemSlots[i].UpdateSlot(inventory[i]);
            }

            for(int i = 0; i < stash.Count; i++)
            {
                stashItemSlots[i].UpdateSlot(stash[i]);
            }
        }

        public void AddItem(ItemDataSO item)
        {
            if (item.itemType == ItemType.Equipment)
                AddToInventory(item);

            else if(item.itemType == ItemType.Material)
                AddToStash(item);

            UpdateSlotUI();
        }

        private void AddToStash(ItemDataSO item)
        {
            if (stashDictionary.TryGetValue(item, out InventoryItem value))
            {
                value.AddStack();
            }
            else
            {
                InventoryItem newItem = new InventoryItem(item);
                stash.Add(newItem);
                stashDictionary.Add(item, newItem);
            }
        }

        private void AddToInventory(ItemDataSO item)
        {
            if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
            {
                value.AddStack();
            }
            else
            {
                InventoryItem newItem = new InventoryItem(item);
                inventory.Add(newItem);
                inventoryDictionary.Add(item, newItem);
            }
        }

        public void RemoveItem(ItemDataSO item)
        {
            if(inventoryDictionary.TryGetValue(item, out InventoryItem value))
            {
                if(value.stackSize <= 1)
                {
                    inventory.Remove(value);
                    inventoryDictionary.Remove(item);
                }
                else
                {
                    value.RemoveStack();
                }
            }

            if(stashDictionary.TryGetValue(item, out InventoryItem stashValue))
            {
                if(stashValue.stackSize <= 1)
                {
                    stash.Remove(stashValue);
                    stashDictionary.Remove(item);
                }
                else
                {
                    stashValue.RemoveStack();
                }
            }

            UpdateSlotUI();
        }

        public bool CanCraft(EquipmentDataSO equipmentToCraft, List<InventoryItem> requiredMaterials)
        {
            List<InventoryItem> materialsToUse = new List<InventoryItem>();

            for(int i = 0; i < requiredMaterials.Count; i++)
            {
                if (stashDictionary.TryGetValue(requiredMaterials[i].itemDataSO, out InventoryItem stashValue))
                {
                    if(stashValue.stackSize >= requiredMaterials[i].stackSize)
                    {
                        // ������ ����� �������� stash�� ������ �ִٸ�
                        // ����ؼ� ���� ��� ����� materialToUse �� �߰�
                        materialsToUse.Add(stashValue);
                    }
                    else
                    {
                        // ������ �ʿ��� ��ᰡ stash�� ������ 
                        // �� ������ ����
                        return false;
                    }
                    
                }
                else
                {
                    // ������ �ʿ��� ��ᰡ stash�� ����
                    return false;
                }
            }

            for(int i = 0; i < materialsToUse.Count; i++)
            {
                RemoveItem(materialsToUse[i].itemDataSO);
            }

            // ������ ��� �κ��丮�� �߰�
            AddItem(equipmentToCraft);
            return true;
        }

        public List<InventoryItem> GetEquipmentList() => equipment;
        public List<InventoryItem> GetStashList() => stash;

        public EquipmentDataSO GetEquipment(EquipmentType type)
        {
            EquipmentDataSO equippedItem = null;

            foreach (KeyValuePair<EquipmentDataSO, InventoryItem> _item in equipmentDictionary)
            {
                if (_item.Key.equipmentType == type)
                {
                    equippedItem = _item.Key;
                }
            }

            return equippedItem;
        }

        public void UsePotion()
        {
            EquipmentDataSO potion = GetEquipment(EquipmentType.Potion);

            if (potion == null)
                return;

            bool canUsePotion = Time.time > lastTimeUsedPotion + potionCooldown;
            if(canUsePotion)
            {
                potionCooldown = potion.itemCoolDown;
                potion.PlayEffect(null);
                lastTimeUsedPotion = Time.time;
            }
            else
            {
                Debug.Log("���� ��ٿ� �� ����� �� �����");
            }
        }

        public bool CanUseArmor()
        {
            EquipmentDataSO armor = GetEquipment(EquipmentType.Armor);

            if (armor == null)
                return false;

            bool canUseArmor = Time.time > lastTimeUsedArmor + armorCooldown;
            if (canUseArmor)
            {
                armorCooldown = armor.itemCoolDown;
                lastTimeUsedArmor = Time.time;
                return true;
            }

            Debug.Log("�Ƹ� ��ٿ� ��");
            return false;
        }
    }   
}
