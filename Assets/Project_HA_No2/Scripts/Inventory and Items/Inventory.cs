using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Manages the player's entire inventory system, including item storage,
    /// equipment management, weapon slot integration, UI synchronization,
    /// and game save/load functionality.
    /// 
    /// Responsibilities:
    /// - Tracks inventory, stash, equipment, and weapon slot items
    /// - Equips and unequips gear, handling visual parts and stat modifiers
    /// - Integrates with the WeaponQuickSlotUI and WeaponInfoUI
    /// - Provides support for crafting, potions, armor usage, and quest tracking
    /// - Supports full game save/load integration through ISaveManager
    /// </summary>
    public class Inventory : SingletonBase<Inventory>, ISaveManager
    {
        private PlayerManager playerManager;
        private EquipmentPrefabManager equipmentPrefabManager;

        /// <summary>
        /// Items given to the player at the beginning of the game.
        /// </summary>
        public List<ItemDataSO> initialItems;


        /// <summary>
        /// Equipment currently equipped by the player.
        /// </summary>
        public List<InventoryItem> equipment;
        /// <summary>
        /// Dictionary for fast access to currently equipped items by EquipmentDataSO.
        /// </summary>
        public Dictionary<EquipmentDataSO, InventoryItem> equipmentDictionary;

        /// <summary>
        /// Weapons currently assigned to quick weapon slots.
        /// </summary>
        public List<InventoryItem> weapons;
        /// <summary>
        /// Lookup table for quick-access weapons by EquipmentDataSO.
        /// </summary>
        public Dictionary<EquipmentDataSO, InventoryItem> weaponDictionary;


        [Header("Inventory Items")]
        /// <summary>
        /// General inventory items (non-stash, non-equipped).
        /// </summary>
        public List<InventoryItem> inventory;
        /// <summary>
        /// Fast lookup for inventory items by their base data.
        /// </summary>
        public Dictionary<ItemDataSO, InventoryItem> inventoryDictionary;


        /// <summary>
        /// Items stored in the stash (e.g., crafting materials).
        /// </summary>
        public List<InventoryItem> stash;
        /// <summary>
        /// Fast lookup for stash items by their base data.
        /// </summary>
        public Dictionary<ItemDataSO, InventoryItem> stashDictionary;

        [Header("InventoryUI")]
        [SerializeField] private Transform inventorySlotParent;
        [SerializeField] private Transform stashSlotParent;
        [SerializeField] private Transform equipmentSlotParent;
        [SerializeField] private Transform weaponSlotParent;
        [SerializeField] private Transform statSlotParent;

        [Header("Weapon Info UI")]
        [SerializeField] private WeaponInfoUI weaponInfoUI;
        [SerializeField] private WeaponQuickSlotUI weaponQuickSlotUI;

        private ItemSlotUI[] inventoryItemSlots;
        private ItemSlotUI[] stashItemSlots;
        private EquipmentSlotUI[] equipmentSlots;
        private EquipmentSlotUI[] weaponSlots;
        private StatSlotUI[] statSlots;



        [Header("Items information")]
        private float lastTimeUsedPotion;
        private float lastTimeUsedArmor;
        public float potionCooldown;
        private float armorCooldown;

        [Header("Data Base")]
        public List<InventoryItem> loadedItems;
        public List<EquipmentDataSO> loadedEquipments;


        /// <summary>
        /// Initializes dictionaries, UI slots, and loads starting or saved items.
        /// </summary>
        private void Start()
        {
            playerManager = PlayerManager.Instance;
            equipmentPrefabManager = FindAnyObjectByType<EquipmentPrefabManager>();

            inventory = new List<InventoryItem>();
            inventoryDictionary = new Dictionary<ItemDataSO, InventoryItem>();

            stash = new List<InventoryItem>();
            stashDictionary = new Dictionary<ItemDataSO, InventoryItem>();

            equipment = new List<InventoryItem>();
            equipmentDictionary = new Dictionary<EquipmentDataSO, InventoryItem>();

            weapons = new List<InventoryItem>();
            weaponDictionary = new Dictionary<EquipmentDataSO, InventoryItem>();

            inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
            stashItemSlots = stashSlotParent.GetComponentsInChildren<ItemSlotUI>();
            equipmentSlots = equipmentSlotParent.GetComponentsInChildren<EquipmentSlotUI>();
            weaponSlots = weaponSlotParent.GetComponentsInChildren<EquipmentSlotUI>();
            statSlots = statSlotParent.GetComponentsInChildren<StatSlotUI>();

            InitializeItems();
        }


        /// <summary>
        /// Loads saved equipment and items or initializes from default list.
        /// </summary>
        private void InitializeItems()
        {
            foreach (EquipmentDataSO equipment in loadedEquipments)
            {
                EquipEquipment(equipment);
            }

            if (loadedItems.Count > 0)
            {
                foreach (InventoryItem item in loadedItems)
                {
                    for (int i = 0; i < item.stackSize; i++)
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

        
        /// <summary>
        /// Registers a weapon into weapon slot tracking and assigns it to a quick slot if needed.
        /// </summary>
        public void AssignWeaponToSlot(EquipmentDataSO newWeapon, RectTransform fromRect = null)
        {
            if (newWeapon == null) return;

            Debug.Log($"[AssignWeaponToSlot] Called with: {newWeapon.name}");


            var existingWeapons = weaponDictionary
                .Where(kvp => (kvp.Value.itemDataSO as EquipmentDataSO).equipmentType == newWeapon.equipmentType)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var existingWeapon in existingWeapons)
            {
                Debug.Log($"[UnAssign] Removing existing weapon: {existingWeapon}");
                weaponDictionary.Remove(existingWeapon);
                weapons.RemoveAll(i => i.itemDataSO == existingWeapon);
            }


            RemoveItem(newWeapon, true);

            var newItem = new InventoryItem(newWeapon);
            weapons.Add(newItem);
            weaponDictionary[newWeapon] = newItem;

            if (!weaponQuickSlotUI.HasWeapon(newWeapon))
                weaponQuickSlotUI.RegisterToFirstEmptySlot(newWeapon);

            
            if (!weaponQuickSlotUI.HasWeapon(newWeapon))
            {
                var targetSlot = weaponQuickSlotUI.RegisterToFirstEmptySlot(newWeapon);

                // 연출은 여기서 시작
                if (fromRect != null && targetSlot != null)
                {
                    UIAnimationUtility.AnimateItemFlyToSlot(
                        from: fromRect,
                        to: targetSlot.iconImage.rectTransform,
                        iconSprite: newWeapon.icon,
                        canvasRoot: CanvasUI.Instance.transform
                    );
                }
            }

            UpdateSlotUI();
        }

        /// <summary>
        /// Equips a piece of equipment, removes existing equipment of same type, and updates visuals.
        /// </summary>
        public void EquipEquipment(ItemDataSO item)
        {
            EquipmentDataSO newEquipment = item as EquipmentDataSO;
            if (newEquipment == null)
                return;

            InventoryItem newItem = new InventoryItem(newEquipment);

            EquipmentDataSO oldEquipment = null;
            foreach (var pair in equipmentDictionary)
            {
                if (pair.Key.equipmentType == newEquipment.equipmentType)
                {
                    oldEquipment = pair.Key;
                    break;
                }
            }

            if (oldEquipment != null)
            {
                UnEquipEquipment(oldEquipment);
                foreach (var part in oldEquipment.parts)
                {
                    equipmentPrefabManager.UnequipPart(part);
                }

                AddItem(oldEquipment);
                equipmentDictionary.Remove(oldEquipment);
                equipment.RemoveAll(i => i.itemDataSO == oldEquipment);
            }

            equipment.Add(newItem);
            equipmentDictionary[newEquipment] = newItem;
            newEquipment.AddModifiers();

            foreach (var part in newEquipment.parts)
            {
                equipmentPrefabManager.EquipPart(part);
            }

            RemoveItem(item);
            UpdateSlotUI();
        }

        /// <summary>
        /// Equips a weapon, removing old one if needed, and updates weapon UI and model.
        /// </summary>
        public void EquipWeapon(EquipmentDataSO newWeapon)
        {
            if (newWeapon == null || newWeapon.equipmentType != EquipmentType.Weapon)
                return;

            EquipmentDataSO oldWeapon = null;

            foreach (var pair in equipmentDictionary)
            {
                if (pair.Key.equipmentType == EquipmentType.Weapon)
                {
                    oldWeapon = pair.Key;
                    break;
                }
            }

            if (oldWeapon != null)
            {
                UnEquipEquipment(oldWeapon);

                foreach (var part in oldWeapon.parts)
                {
                    equipmentPrefabManager.UnequipPart(part);
                }

                // Return old weapon to inventory
                AddItem(oldWeapon);
                equipmentDictionary.Remove(oldWeapon);
                equipment.RemoveAll(i => i.itemDataSO == oldWeapon);
            }

            // Add new weapon
            var newItem = new InventoryItem(newWeapon);
            equipment.Add(newItem);
            equipmentDictionary[newWeapon] = newItem;
            newWeapon.AddModifiers();

            foreach (var part in newWeapon.parts)
            {
                equipmentPrefabManager.EquipPart(part);
            }

            var weaponHandler = playerManager.playerCharacter.weaponHandler;
            weaponHandler.SetWeapon(newWeapon);
            weaponInfoUI.Bind(weaponHandler.GetViewModel());
        }

        /// <summary>
        /// Unequips a general equipment item (non-visual).
        /// </summary>
        public void UnEquipEquipment(EquipmentDataSO itemToRemove)
        {
            if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
            {
                equipment.Remove(value);
                equipmentDictionary.Remove(itemToRemove);
                itemToRemove.RemoveModifiers();
            }
        }

        
        /// <summary>
        /// Unequips a weapon, removes it from quick slot and re-adds it to inventory.
        /// </summary>
        public void UnEquipWeapon(EquipmentDataSO weaponToRemove)
        {

            if (weaponToRemove == null) return;

            // 1. weaponDictionary에서 제거
            if (weaponDictionary.ContainsKey(weaponToRemove))
            {
                weaponDictionary.Remove(weaponToRemove);
                weapons.RemoveAll(i => i.itemDataSO.itemID == weaponToRemove.itemID);

                Debug.Log($"[UnEquipWeapon] Removed {weaponToRemove.name} from weaponDictionary");
            }

            // 2. 퀵슬롯 UI에서도 제거
            weaponQuickSlotUI.RemoveWeapon(weaponToRemove);

            // 3. 인벤토리에 다시 추가
            AddItem(weaponToRemove);

            // 4. UI 갱신
            UpdateSlotUI();

        }

        
        /// <summary>
        /// Refreshes all UI slots (equipment, inventory, stash, stat).
        /// </summary>
        private void UpdateSlotUI()
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                foreach (KeyValuePair<EquipmentDataSO, InventoryItem> _item in equipmentDictionary)
                {
                    if (_item.Key.equipmentType == equipmentSlots[i].equipmentSlotType)
                    {
                        equipmentSlots[i].UpdateSlot(_item.Value);
                        break;
                    }
                }
            }

            foreach (var weapon in weaponDictionary)
            {
                var weaponItem = weapon.Value;
                var weaponData = weaponItem.itemDataSO as EquipmentDataSO;

                for (int i = 0; i < weaponSlots.Length; i++)
                {
                    if (!weaponSlots[i].isUsing &&
                        weaponData.equipmentType == weaponSlots[i].equipmentSlotType)
                    {
                        weaponSlots[i].UpdateSlot(weaponItem);
                        break;
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


        /// <summary>
        /// Updates the UI for all stat slots.
        /// </summary>
        public void UptateStatUI()
        {
            for (int i = 0; i < statSlots.Length; i++)
            {
                statSlots[i].UpdateStatValueUI();
            }
        }


        /// <summary>
        /// Adds an item to inventory, stash, or applies money effect. Also triggers collection event.
        /// </summary>
        public void AddItem(ItemDataSO item, int stackCount = 1)
        {
            if (item.itemType == ItemType.Equipment && CanAddItemToInventory())
            {
                for (int i = 0; i < stackCount; i++)
                    AddToInventory(item);
            }

            else if (item.itemType == ItemType.Material)
                AddToStash(item);

            else if (item.itemType == ItemType.Money)
            {
                MoneySO money = item as MoneySO;
                playerManager.currency += money.moneyAmount;
            }

            EventBus.Instance.Publish(new GameEvent.ItemCollected(item.itemID));


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


        /// <summary>
        /// Removes item from inventory or stash. Optionally skips UI update.
        /// </summary>
        public void RemoveItem(ItemDataSO item, bool skipUpdate = false)
        {
            if (inventoryDictionary.TryGetValue(item, out InventoryItem value))
            {
                if (value.stackSize <= 1)
                {
                    inventory.Remove(value);
                    inventoryDictionary.Remove(item);
                }
                else
                {
                    value.RemoveStack();
                }
            }

            if (stashDictionary.TryGetValue(item, out InventoryItem stashValue))
            {
                if (stashValue.stackSize <= 1)
                {
                    stash.Remove(stashValue);
                    stashDictionary.Remove(item);
                }
                else
                {
                    stashValue.RemoveStack();
                }
            }

            if (!skipUpdate)
                UpdateSlotUI();


        }

        public bool CanAddItemToInventory()
        {
            if (inventory.Count >= inventoryItemSlots.Length)
            {
                return false;
            }

            return true;
        }

        public bool CanCraft(EquipmentDataSO equipmentToCraft, List<InventoryItem> requiredMaterials)
        {
            List<InventoryItem> materialsToUse = new List<InventoryItem>();

            for (int i = 0; i < requiredMaterials.Count; i++)
            {
                if (stashDictionary.TryGetValue(requiredMaterials[i].itemDataSO, out InventoryItem stashValue))
                {
                    if (stashValue.stackSize >= requiredMaterials[i].stackSize)
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

            for (int i = 0; i < materialsToUse.Count; i++)
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
            if (canUsePotion)
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

        public void LoadData(GameData _data)
        {
            foreach (KeyValuePair<string, int> pair in _data.inventory)
            {
                foreach (var item in GetItemDataBase())
                {
                    if (item != null && item.itemID == pair.Key)
                    {
                        InventoryItem itemToLoad = new InventoryItem(item);
                        itemToLoad.stackSize = pair.Value;

                        loadedItems.Add(itemToLoad);
                    }
                }
            }

            foreach (string loadedItemId in _data.equipmentId)
            {
                foreach (var item in GetItemDataBase())
                {
                    if (item != null && loadedItemId == item.itemID)
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


            foreach (KeyValuePair<ItemDataSO, InventoryItem> pair in inventoryDictionary)
            {
                Debug.Log("Saving inventory...");
                Debug.Log($"Inventory count: {inventoryDictionary.Count}");
                _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
            }

            foreach (KeyValuePair<ItemDataSO, InventoryItem> pair in stashDictionary)
            {
                _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
            }

            foreach (KeyValuePair<EquipmentDataSO, InventoryItem> pair in equipmentDictionary)
            {
                _data.equipmentId.Add(pair.Key.itemID);
            }
        }

        private List<ItemDataSO> GetItemDataBase()
        {
            List<ItemDataSO> itemDataBase = new List<ItemDataSO>();
            string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Project_HA_No2/ScriptableObjects" });

            foreach (string SOName in assetNames)
            {
                var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
                var itemData = AssetDatabase.LoadAssetAtPath<ItemDataSO>(SOPath);
                itemDataBase.Add(itemData);
            }

            return itemDataBase;
        }
    }   
}
