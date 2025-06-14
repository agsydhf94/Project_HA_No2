using UnityEngine;

namespace HA
{
    /// <summary>
    /// Handles input for managing and switching weapon quick slots.
    /// Supports scroll-based navigation and popup UI interaction.
    /// </summary>
    public class WeaponQuickSlotInput : MonoBehaviour
    {
        /// <summary>
        /// Reference to the popup UI for visualizing quick slot selection.
        /// </summary>
        [SerializeField] private WeaponQuickSlotPopup popupUI;

        /// <summary>
        /// Reference to the UI logic for managing quick slot selection.
        /// </summary>
        [SerializeField] private WeaponQuickSlotUI quickSlotUI;

        /// <summary>
        /// Whether the player is currently in quick slot selection mode.
        /// </summary>
        private bool isInQuickSlotMode;


        /// <summary>
        /// Initializes references to popup and UI components.
        /// </summary>
        void Awake()
        {
            popupUI = GetComponent<WeaponQuickSlotPopup>();
            quickSlotUI = GetComponent<WeaponQuickSlotUI>();
        }


        /// <summary>
        /// Checks for input to enter quick slot mode, scroll through slots, or confirm selection.
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                EnterQuickSlotMode();
            }

            if (isInQuickSlotMode)
            {
                // Scroll wheel input to switch between non-empty slots
                float scroll = Input.GetAxis("Mouse ScrollWheel");

                if (scroll != 0f)
                {
                    int direction = scroll > 0f ? 1 : -1;
                    TryScrollSlot(direction);
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                    ExitQuickSlotMode(false);
            }

            // Confirm selection when 'C' is released
            if (Input.GetKeyUp(KeyCode.C))
            {
                ExitQuickSlotMode(true);
            }
        }


        /// <summary>
        /// Attempts to scroll to a valid weapon slot in the given direction.
        /// Skips empty slots.
        /// </summary>
        /// <param name="direction">1 for forward scroll, -1 for backward.</param>
        private void TryScrollSlot(int direction)
        {
            int maxSlots = 2;
            int startIndex = quickSlotUI.currentSlotIndex;
            int nextIndex = startIndex;

            for (int i = 0; i < maxSlots; i++)
            {
                nextIndex = (nextIndex + direction + maxSlots) % maxSlots;

                var weapon = quickSlotUI.GetWeaponInSlot(nextIndex);
                if (weapon != null)
                {
                    quickSlotUI.SwitchSlot(nextIndex);
                    return;
                }
            }

            // Do nothing if all slots are empty
        }


        /// <summary>
        /// Enters quick slot selection mode and opens the popup UI.
        /// </summary>
        void EnterQuickSlotMode()
        {
            isInQuickSlotMode = true;
            popupUI.Open(null); // null if just showing slot UI
        }


        /// <summary>
        /// Exits quick slot selection mode and optionally confirms the selected slot.
        /// </summary>
        /// <param name="confirm">If true, the selected weapon is equipped.</param>
        void ExitQuickSlotMode(bool confirm)
        {
            isInQuickSlotMode = false;

            if (confirm)
            {
                quickSlotUI.ConfirmAndEquipWeapon();
            }

            popupUI.Close();
        }
    }
}
