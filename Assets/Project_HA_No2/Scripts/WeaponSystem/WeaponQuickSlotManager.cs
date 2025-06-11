using UnityEngine;

namespace HA
{
    /// <summary>
    /// Manages quick weapon slots for the player, allowing weapon registration and switching between slots.
    /// </summary>
    public class WeaponQuickSlotManager : MonoBehaviour
    {
        /// <summary>
        /// Array holding the weapon data assigned to each quick slot.
        /// </summary>
        public EquipmentDataSO[] quickSlots = new EquipmentDataSO[2];

        /// <summary>
        /// Index of the currently selected weapon slot.
        /// </summary>
        public int currentSlotIndex = 0;

        /// <summary>
        /// Reference to the player's WeaponHandler for equipping weapons.
        /// </summary>
        private WeaponHandler weaponHandler;


        /// <summary>
        /// Initializes the weapon handler reference and equips the weapon in the current slot.
        /// </summary>
        private void Start()
        {
            weaponHandler = PlayerManager.Instance.playerCharacter.weaponHandler;
            UpdateEquippedWeapon();
        }


        /// <summary>
        /// Registers a weapon to the specified quick slot.
        /// If the slot is currently active, the weapon is equipped immediately.
        /// </summary>
        /// <param name="weapon">The weapon data to assign.</param>
        /// <param name="slotIndex">The index of the quick slot (0 or 1).</param>
        public void RegisterWeaponToSlot(EquipmentDataSO weapon, int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= quickSlots.Length)
                return;

            quickSlots[slotIndex] = weapon;

            // If this is the active slot, update the equipped weapon immediately
            if (slotIndex == currentSlotIndex)
            {
                UpdateEquippedWeapon();
            }
        }


        /// <summary>
        /// Switches the current weapon slot and equips the weapon in the new slot.
        /// </summary>
        /// <param name="newIndex">The index of the new slot to switch to.</param>
        public void SwitchSlot(int newIndex)
        {
            if (newIndex < 0 || newIndex >= quickSlots.Length) return;

            currentSlotIndex = newIndex;
            UpdateEquippedWeapon();
        }


        /// <summary>
        /// Equips the weapon from the currently selected slot using the WeaponHandler.
        /// </summary>
        private void UpdateEquippedWeapon()
        {
            var selectedWeapon = quickSlots[currentSlotIndex];
            if (selectedWeapon != null)
            {
                weaponHandler.SetWeapon(selectedWeapon);
            }
        }


        /// <summary>
        /// Returns the currently equipped weapon data.
        /// </summary>
        /// <returns>The EquipmentDataSO in the active slot.</returns>
        public EquipmentDataSO GetCurrentWeapon() => quickSlots[currentSlotIndex];
    }
}
