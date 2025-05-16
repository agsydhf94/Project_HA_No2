using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace HA
{
    public class Inventory : SingletonBase<Inventory>, ISaveManager
    {
        private EquipmentPrefabManager equipmentPrefabManager;

        public List<ItemDataSO> initialItems;

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
        [SerializeField] private Transform statSlotParent;

        private ItemSlotUI[] inventoryItemSlots;
        private ItemSlotUI[] stashItemSlots;
        private EquipmentSlotUI[] equipmentSlots;
        private StatSlotUI[] statSlots;

        [Header("Items information")]
        private float lastTimeUsedPotion;
        private float lastTimeUsedArmor;
        public float potionCooldown;
        private float armorCooldown;

        [Header("Data Base")]
        public List<InventoryItem> loadedItems;
        public List<EquipmentDataSO> loadedEquipments;

        private void Start()
        {
            equipmentPrefabManager = FindAnyObjectByType<EquipmentPrefabManager>();

            inventory = new List<InventoryItem>();
            inventoryDictionary = new Dictionary<ItemDataSO, InventoryItem>();

            stash = new List<InventoryItem>();
            stashDictionary = new Dictionary<ItemDataSO, InventoryItem>();

            equipment = new List<InventoryItem>();
            equipmentDictionary = new Dictionary<EquipmentDataSO, InventoryItem>();

            inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
            stashItemSlots = stashSlotParent.GetComponentsInChildren<ItemSlotUI>();
            equipmentSlots = equipmentSlotParent.GetComponentsInChildren<EquipmentSlotUI>();
            statSlots = statSlotParent.GetComponentsInChildren<StatSlotUI>();

            InitializeItems();
        }

        private void Update()
        {
            
        }

        private void InitializeItems()
        {
            foreach (EquipmentDataSO equipment in loadedEquipments)
            {
                EquipEquipment(equipment);
            }

            if (loadedItems.Count > 0)
            {
                foreach(InventoryItem item in loadedItems)
                {
                    for(int i = 0; i < item.stackSize; i++)
                    {
                        AddItem(item.itemDataSO);
                    }
                }

                return;
            }

            for (int i = 0; i < initialItems.Count; i++)
            {
                if (initialItems[i] != null)
                {
                    AddItem(initialItems[i]);
                }                
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
                // 이미 장착된 장비(oldEquipment)가 있으면 Equipment 창에선 사라져야하고
                UnEquipEquipment(oldEquipment);

                // 그리고 들고 있던 프리팹도 없애야 한다
                foreach(var part in oldEquipment.parts)
                {
                    equipmentPrefabManager.UnequipPart(part);
                }

                // 해당 장비는 새로 장착될 무기에 의해 다시 인벤토리로 돌아가야 한다.
                AddItem(oldEquipment);
            }

            // 이미 장착된 장비(oldEquipment)가 없다면
            // 현재 장착된 장비를 관리하는 곳(equipment, equipmentDictionary)에 정보를 넣고
            // 인벤토리에선 지운다
            equipment.Add(newItem);
            equipmentDictionary.Add(newEquipment, newItem);
            newEquipment.AddModifiers();

            foreach(var part in newEquipment.parts)
            {
                equipmentPrefabManager.EquipPart(part);
            }

            RemoveItem(item);

            UpdateSlotUI();
        }

        public void UnEquipEquipment(EquipmentDataSO itemToRemove)
        {
            if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
            {
                foreach (var part in itemToRemove.parts)
                {
                    equipmentPrefabManager.UnequipPart(part);
                }

                equipment.Remove(value);
                equipmentDictionary.Remove(itemToRemove);
                itemToRemove.RemoveModifiers();
            }
        }

        // 아이템 픽업이나 추가할 때 이 메서드를 호출
        private void UpdateSlotUI()
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                foreach (KeyValuePair<EquipmentDataSO, InventoryItem> _item in equipmentDictionary)
                {
                    if (_item.Key.equipmentType == equipmentSlots[i].equipmentSlotType)
                    {
                        equipmentSlots[i].UpdateSlot(_item.Value);
                    }
                }
            }

            for (int i = 0; i < inventoryItemSlots.Length; i++)
            {
                inventoryItemSlots[i].CleanUpSlot();
            }
            for (int i = 0; i < stashItemSlots.Length; i++)
            {
                stashItemSlots[i].CleanUpSlot();
            }


            for (int i = 0; i < inventory.Count; i++)
            {
                inventoryItemSlots[i].UpdateSlot(inventory[i]);
            }

            for (int i = 0; i < stash.Count; i++)
            {
                stashItemSlots[i].UpdateSlot(stash[i]);
            }

            UptateStatUI();
        }

        public void UptateStatUI()
        {
            for (int i = 0; i < statSlots.Length; i++)
            {
                statSlots[i].UpdateStatValueUI();
            }
        }

        public void AddItem(ItemDataSO item)
        {
            if (item.itemType == ItemType.Equipment && CanAddItemToInventory())
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

        public bool CanAddItemToInventory()
        {
            if(inventory.Count >= inventoryItemSlots.Length)
            {
                return false;
            }

            return true;
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
                        // 생성에 충분한 아이템을 stash에 가지고 있다면
                        // 사용해서 없앨 재료 목록인 materialToUse 에 추가
                        materialsToUse.Add(stashValue);
                    }
                    else
                    {
                        // 생성에 필요한 재료가 stash에 있지만 
                        // 그 개수가 부족
                        return false;
                    }
                    
                }
                else
                {
                    // 생성에 필요한 재료가 stash에 없음
                    return false;
                }
            }

            for(int i = 0; i < materialsToUse.Count; i++)
            {
                RemoveItem(materialsToUse[i].itemDataSO);
            }

            // 생성한 장비를 인벤토리에 추가
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
                Debug.Log("포션 쿨다운 중 사용할 수 없어요");
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

            Debug.Log("아머 쿨다운 중");
            return false;
        }

        public void LoadData(GameData _data)
        {
            foreach (KeyValuePair<string, int> pair in _data.inventory)
            {
                foreach(var item in GetItemDataBase())
                {
                    if(item != null && item.itemId == pair.Key)
                    {
                        InventoryItem itemToLoad = new InventoryItem(item);
                        itemToLoad.stackSize = pair.Value;

                        loadedItems.Add(itemToLoad);
                    }
                }
            }

            foreach(string loadedItemId in _data.equipmentId)
            {
                foreach(var item in GetItemDataBase())
                {
                    if(item != null && loadedItemId == item.itemId)
                    {
                        loadedEquipments.Add(item as EquipmentDataSO);
                    }
                }
            }
        }

        public void SaveData(ref GameData _data)
        {
            _data.inventory.Clear();
            _data.equipmentId.Clear();
            

            foreach(KeyValuePair<ItemDataSO, InventoryItem> pair in inventoryDictionary)
            {
                Debug.Log("Saving inventory...");
                Debug.Log($"Inventory count: {inventoryDictionary.Count}");
                _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
            }

            foreach(KeyValuePair<ItemDataSO, InventoryItem> pair in stashDictionary)
            {
                _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
            }

            foreach(KeyValuePair<EquipmentDataSO, InventoryItem> pair in equipmentDictionary)
            {
                _data.equipmentId.Add(pair.Key.itemId);
            }
        }

        private List<ItemDataSO> GetItemDataBase()
        {
            List<ItemDataSO> itemDataBase = new List<ItemDataSO>();
            string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Project_HA_No2/ScriptableObjects" });

            foreach(string SOName in assetNames)
            {
                var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
                var itemData = AssetDatabase.LoadAssetAtPath<ItemDataSO>(SOPath);
                itemDataBase.Add(itemData);
            }

            return itemDataBase;
        }
    }   
}
