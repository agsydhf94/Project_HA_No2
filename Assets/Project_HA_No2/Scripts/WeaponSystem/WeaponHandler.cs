using UnityEngine;

namespace HA
{
    /// <summary>
    /// Handles the player's currently equipped weapon, including setting, firing, reloading, and updating UI.
    /// </summary>
    public class WeaponHandler : MonoBehaviour
    {
        // Main Weapon

        /// <summary>
        /// The currently equipped main weapon instance.
        /// </summary>
        private IWeapon currentMainWeapon;

        /// <summary>
        /// The data associated with the current main weapon.
        /// </summary>
        private EquipmentDataSO currentMainWeaponData;

        /// <summary>
        /// ViewModel for displaying main weapon bullet and meta information in UI.
        /// </summary>
        private GunViewModel mainWeaponViewModel = new();


        // Sub Weapon

        /// <summary>
        /// The currently equipped sub weapon instance.
        /// </summary>
        private IWeapon currentSubWeapon;

        /// <summary>
        /// The data associated with the current sub weapon.
        /// </summary>
        private EquipmentDataSO currentSubWeaponData;

        /// <summary>
        /// ViewModel for displaying sub weapon bullet and meta information in UI.
        /// </summary>
        private GunViewModel subWeaponViewModel = new();


        /// <summary>
        /// Reference to the weapon ammo status tracker.
        /// </summary>
        private WeaponAmmoStatus weaponAmmoStatus;

        /// <summary>
        /// Reference to the player character script for animation handling.
        /// </summary>
        private PlayerCharacter playerCharacter;

        private void Awake()
        {
            weaponAmmoStatus = GetComponent<WeaponAmmoStatus>();
            playerCharacter = GetComponent<PlayerCharacter>();
        }

        /// <summary>
        /// Sets the equipped weapon (main or sub) based on the given EquipmentData.
        /// </summary>
        /// <param name="equipmentData">The weapon data to set.</param>
        public void SetWeapon(EquipmentDataSO equipmentData)
        {
            if (equipmentData == null) return;

            switch (equipmentData.equipmentType)
            {
                case EquipmentType.Weapon:
                    SetMainWeapon(equipmentData);
                    break;
                case EquipmentType.SubWeapon:
                    SetSubWeapon(equipmentData);
                    break;
            }
        }


        /// <summary>
        /// Equips the specified main weapon and initializes related data and UI.
        /// </summary>
        private void SetMainWeapon(EquipmentDataSO data)
        {
            if (data == currentMainWeaponData) return;

            currentMainWeaponData = data;

            IWeapon weapon = FindWeaponInstance(data);
            currentMainWeapon = weapon;
            HandleArmingAnimation(weapon);
            weapon.InitializeWeaponData(data);

            if (weapon is IAmmoWeapon ammoWeapon)
            {
                ammoWeapon.InjectAmmoStatus(weaponAmmoStatus);
                mainWeaponViewModel.SetBullet(ammoWeapon.TransferBulletData());
            }

            mainWeaponViewModel.SetMeta(data.TransferWeaponMetaData());
        }


        /// <summary>
        /// Equips the specified sub weapon and initializes related data and UI.
        /// </summary>
        private void SetSubWeapon(EquipmentDataSO data)
        {
            if (data == currentSubWeaponData) return;

            currentSubWeaponData = data;

            IWeapon weapon = FindWeaponInstance(data);
            currentSubWeapon = weapon;
            weapon.InitializeWeaponData(data);

            if (weapon is IAmmoWeapon ammoWeapon)
            {
                ammoWeapon.InjectAmmoStatus(weaponAmmoStatus);
                subWeaponViewModel.SetBullet(ammoWeapon.TransferBulletData());
            }

            subWeaponViewModel.SetMeta(data.TransferWeaponMetaData());
        }


        /// <summary>
        /// Finds the weapon instance in the scene using the weapon type tag.
        /// </summary>
        /// <param name="data">The equipment data containing the weapon type.</param>
        /// <returns>The IWeapon instance found in the scene.</returns>
        private IWeapon FindWeaponInstance(EquipmentDataSO data)
        {
            GameObject weaponObject = GameObject.FindGameObjectWithTag(data.weaponType.ToString());
            return weaponObject?.GetComponent<IWeapon>();
        }


        /// <summary>
        /// Plays the appropriate character arming animation based on the weapon type.
        /// </summary>
        private void HandleArmingAnimation(IWeapon weapon)
        {
            if (weapon is RifleWeapon || weapon is MissileLauncher)
                playerCharacter.CharacterRifleArmed();
            else if (weapon is SwordWeapon)
                playerCharacter.CharacterSwordArmed();
        }


        /// <summary>
        /// Clears the reference to the currently equipped main weapon.
        /// </summary>
        public void ClearMainWeapon()
        {
            currentMainWeapon = null;
            currentMainWeaponData = null;
        }


        /// <summary>
        /// Clears the reference to the currently equipped sub weapon.
        /// </summary>
        public void ClearSubWeapon()
        {
            currentSubWeapon = null;
            currentSubWeaponData = null;
        }


        /// <summary>
        /// Returns the ViewModel for the main weapon (for UI binding).
        /// </summary>
        public GunViewModel GetMainWeaponViewModel() => mainWeaponViewModel;

        /// <summary>
        /// Returns the ViewModel for the sub weapon (for UI binding).
        /// </summary>
        public GunViewModel GetSubWeaponViewModel() => subWeaponViewModel;


        /// <summary>
        /// Gets the EquipmentData for the current main weapon.
        /// </summary>
        public EquipmentDataSO GetCurrentMainWeaponData() => currentMainWeaponData;

        /// <summary>
        /// Gets the EquipmentData for the current sub weapon.
        /// </summary>
        public EquipmentDataSO GetCurrentSubWeaponData() => currentSubWeaponData;


        /// <summary>
        /// Triggers the main weapon's attack logic.
        /// </summary>
        public void TriggerMainAttack() => currentMainWeapon?.Attack();

        /// <summary>
        /// Triggers the sub weapon's attack logic.
        /// </summary>
        public void TriggerSubAttack() => currentSubWeapon?.Attack();


        /// <summary>
        /// Triggers the reload function if the main weapon supports reloading.
        /// </summary>
        public void TriggerReload()
        {
            if (currentMainWeapon is IReloadeable reloadable)
                reloadable.Reload();
        }


        /// <summary>
        /// Updates the main weapon ViewModel with the latest ammo data.
        /// </summary>
        public void NotifyMainAmmoChanged(IAmmoWeapon ammoWeapon)
        {
            mainWeaponViewModel.SetBullet(ammoWeapon.TransferBulletData());
        }


        /// <summary>
        /// Updates the sub weapon ViewModel with the latest ammo data.
        /// </summary>
        public void NotifySubAmmoChanged(IAmmoWeapon ammoWeapon)
        {
            subWeaponViewModel.SetBullet(ammoWeapon.TransferBulletData());
        }
    }
}

