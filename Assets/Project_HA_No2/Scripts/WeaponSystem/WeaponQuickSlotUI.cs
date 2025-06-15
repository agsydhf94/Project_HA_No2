using UnityEngine;

namespace HA
{
    /// <summary>
    /// Manages the UI and logic for weapon quick slots.
    /// Handles weapon registration, switching, equipping, and unequipping,
    /// and keeps the UI in sync with the player's current weapon state.
    /// </summary>
    public class WeaponQuickSlotUI : MonoBehaviour
    {
         /// <summary>
        /// UI slot cells that display weapon icons and selection frames.
        /// </summary>
        [Header("UI Slots")]
        [SerializeField] private WeaponQuickSlotCellUI[] uiSlots;

        /// <summary>
        /// Index of the currently selected quick slot.
        /// </summary>
        public int currentSlotIndex = 0;

        /// <summary>
        /// Reference to the player's WeaponHandler used to equip weapons.
        /// </summary>
        private WeaponHandler weaponHandler;

        /// <summary>
        /// Reference to the player's inventory system.
        /// </summary>
        private Inventory inventory;


        /// <summary>
        /// Initializes weapon handler and equips weapon stored in the currently selected slot.
        /// </summary>
        private void Start()
        {
            weaponHandler = PlayerManager.Instance.playerCharacter.weaponHandler;
            inventory = Inventory.Instance;
            UpdateEquippedWeapon();
        }


        /// <summary>
        /// Assigns a weapon to the specified quick slot and updates the weapon if it's the active slot.
        /// </summary>
        /// <param name="weapon">The weapon to assign.</param>
        /// <param name="slotIndex">Slot index (0 or 1).</param>
        public void RegisterWeaponToSlot(EquipmentDataSO weapon, int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= uiSlots.Length || weapon == null)
                return;

            // 이미 등록되어 있으면 덮어쓰지 않음
            if (uiSlots[slotIndex].GetWeaponData() == weapon)
                return;

            uiSlots[slotIndex].UpdateSlot(weapon);

            if (slotIndex == currentSlotIndex)
                UpdateEquippedWeapon();
        }


        /// <summary>
        /// Automatically registers the weapon to the first empty quick slot.
        /// </summary>
        /// <param name="weapon">Weapon to assign.</param>
        public void RegisterToFirstEmptySlot(EquipmentDataSO weapon)
        {
            for (int i = 0; i < uiSlots.Length; i++)
            {
                if (uiSlots[i].GetWeaponData() == null)
                {
                    RegisterWeaponToSlot(weapon, i);
                    return;
                }
            }

            Debug.LogWarning("[QuickSlot] No empty slot available.");
        }

        /// <summary>
        /// Switches the current quick slot selection and updates the equipped weapon.
        /// </summary>
        /// <param name="newIndex">Slot index to switch to.</param>
        public void SwitchSlot(int newIndex)
        {
            if (newIndex < 0 || newIndex >= uiSlots.Length)
                return;

            currentSlotIndex = newIndex;
            UpdateEquippedWeapon();
        }

        /// <summary>
        /// Equips the weapon in the current slot and updates the inventory and UI state.
        /// Called when quick slot selection is confirmed.
        /// </summary>
        public void ConfirmAndEquipWeapon()
        {
            var selectedWeapon = uiSlots[currentSlotIndex].GetWeaponData();

            if (selectedWeapon != null)
            {
                inventory.EquipWeapon(selectedWeapon);
                weaponHandler.SetWeapon(selectedWeapon);
            }
            else
            {
                if (weaponHandler.GetCurrentWeaponData() != null)
                {
                    inventory.UnEquipWeapon(selectedWeapon);
                }
                      
                weaponHandler.ClearWeapon();
            }

            // UI 선택 프레임 갱신
            for (int i = 0; i < uiSlots.Length; i++)
                uiSlots[i].SetEquipped(i == currentSlotIndex);
        }

        /// <summary>
        /// Unequips the weapon in the current slot and clears it from both the inventory and UI.
        /// </summary>
        public void UnequipCurrentWeapon()
        {
            var currentWeapon = uiSlots[currentSlotIndex].GetWeaponData();

            if (currentWeapon != null)
            {
                inventory.UnEquipWeapon(currentWeapon); // 실제 장비 해제
                uiSlots[currentSlotIndex].ClearSlot();  // UI에서 제거
            }

            weaponHandler.ClearWeapon(); // 무기 모델 및 핸들러 해제 (필요 시)

            // UI 슬롯 시각적으로 갱신
            uiSlots[currentSlotIndex].SetEquipped(false);
        }

        /// <summary>
        /// Removes a specific weapon from all quick slots.
        /// </summary>
        /// <param name="weapon">The weapon to remove.</param>
        public void RemoveWeapon(EquipmentDataSO weapon)
        {
            for (int i = 0; i < uiSlots.Length; i++)
            {
                if (uiSlots[i].GetWeaponData() == weapon)
                {
                    uiSlots[i].ClearSlot();

                    // 선택 중이던 슬롯에서 제거된 경우 무기 해제
                    if (i == currentSlotIndex)
                    {
                        weaponHandler.ClearWeapon();
                        uiSlots[i].SetEquipped(false);
                    }

                    return;
                }
            }
        }

        /// <summary>
        /// Updates the UI selection frame to reflect the currently active quick slot.
        /// </summary>
        private void UpdateEquippedWeapon()
        {
            for (int i = 0; i < uiSlots.Length; i++)
            {
                bool isSelected = i == currentSlotIndex;
                uiSlots[i].SetEquipped(isSelected);
            }
        }

        
        /// <summary>
        /// Checks if the given weapon is currently equipped.
        /// </summary>
        /// <param name="weaponToCheck">Weapon item to check.</param>
        /// <returns>True if the weapon is in use; otherwise false.</returns>
        public bool CheckInUse(InventoryItem weaponToCheck)
        {
            bool result = false;

            if (weaponHandler.GetCurrentWeaponData() != null)
            {
                if (weaponToCheck.itemDataSO.itemID == weaponHandler.GetCurrentWeaponData().itemID)
                {
                    result = true;
                }
            }

            return result;
        }


        /// <summary>
        /// Gets the weapon data of the currently selected slot.
        /// </summary>
        /// <returns>The currently selected EquipmentDataSO.</returns>
        public EquipmentDataSO GetCurrentWeapon()
        {
            return uiSlots[currentSlotIndex].GetWeaponData();
        }


        /// <summary>
        /// Returns the weapon in the specified slot.
        /// </summary>
        /// <param name="index">Slot index.</param>
        /// <returns>Weapon assigned to the slot, or null if empty.</returns>
        public EquipmentDataSO GetWeaponInSlot(int index)
        {
            if (index < 0 || index >= uiSlots.Length)
                return null;

            return uiSlots[index].GetWeaponData();
        }

        
        /// <summary>
        /// Checks whether a given weapon is registered in any quick slot.
        /// </summary>
        /// <param name="weapon">The weapon to check.</param>
        /// <returns>True if the weapon exists in any slot; otherwise false.</returns>
        public bool HasWeapon(EquipmentDataSO weapon)
        {
            return GetWeaponInSlot(0) == weapon || GetWeaponInSlot(1) == weapon;
        }
    }
}
