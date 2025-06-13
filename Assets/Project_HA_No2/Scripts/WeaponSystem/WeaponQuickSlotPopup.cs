using UnityEngine;
using DG.Tweening;

namespace HA
{
    /// <summary>
    /// Controls the popup UI for assigning a weapon to a quick slot.
    /// Provides animation and interaction logic using DOTween.
    /// </summary>
    public class WeaponQuickSlotPopup : MonoBehaviour
    {
        /// <summary>
        /// CanvasGroup used to control popup visibility and interactivity.
        /// </summary>
        [SerializeField] private CanvasGroup popupCanvasGroup;

        /// <summary>
        /// The weapon that will be registered to a quick slot.
        /// </summary>
        private EquipmentDataSO weaponToRegister;

        /// <summary>
        /// Reference to the quick slot UI that handles slot registration.
        /// </summary>
        private WeaponQuickSlotUI quickSlotUI;


        /// <summary>
        /// Initializes the popup UI and disables its interactivity at startup.
        /// </summary>
        private void Awake()
        {
            quickSlotUI = GetComponent<WeaponQuickSlotUI>();
            popupCanvasGroup.alpha = 0;
            popupCanvasGroup.interactable = false;
            popupCanvasGroup.blocksRaycasts = false;
        }


        /// <summary>
        /// Opens the popup and plays scale and fade-in animations.
        /// </summary>
        /// <param name="weapon">The weapon to register when a slot is selected.</param>
        public void Open(EquipmentDataSO weapon)
        {
            weaponToRegister = weapon;

            popupCanvasGroup.DOFade(1f, 0.3f);
            popupCanvasGroup.interactable = true;
            popupCanvasGroup.blocksRaycasts = true;
            transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero).SetEase(Ease.OutBack);
        }


        /// <summary>
        /// Closes the popup and plays fade-out animation.
        /// </summary>
        public void Close()
        {
            popupCanvasGroup.DOFade(0f, 0.2f);
            popupCanvasGroup.interactable = false;
            popupCanvasGroup.blocksRaycasts = false;
        }


        /// <summary>
        /// Registers the selected weapon to the specified quick slot and closes the popup.
        /// </summary>
        /// <param name="slotIndex">Index of the quick slot (e.g., 0 or 1).</param>
        public void RegisterToSlot(int slotIndex)
        {
            quickSlotUI.RegisterWeaponToSlot(weaponToRegister, slotIndex);
            Close();
        }


        /// <summary>
        /// Shortcut for registering the weapon to quick slots.
        /// </summary>
        public void OnClickSlot1() => RegisterToSlot(0);
        public void OnClickSlot2() => RegisterToSlot(1);
    }
}
