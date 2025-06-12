using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Serializable structure that stores ammo information for a specific weapon.
    /// </summary>
    [System.Serializable]
    public class WeaponSaveData
    {
        /// <summary>
        /// Unique identifier of the weapon.
        /// </summary>
        public string weaponID;

        /// <summary>
        /// Current number of bullets in the magazine.
        /// </summary>
        public int magazineCurrent;

        /// <summary>
        /// Total remaining bullets in reserve.
        /// </summary>
        public int totalAmmo;
    }

    /// <summary>
    /// Tracks ammo status for all weapons equipped by the player.
    /// Supports saving, retrieving, and querying ammo counts.
    /// </summary>
    public class WeaponAmmoStatus : MonoBehaviour
    {
        /// <summary>
        /// Reference to the weapon data database to fetch default values.
        /// </summary>
        [SerializeField] private WeaponDataBaseSO weaponDataBase;

        /// <summary>
        /// Serializable dictionary mapping weapon IDs to their current ammo state.
        /// Enables Unity-friendly persistence and editor visibility if needed.
        /// </summary>
        private SerializableDictionary<string, WeaponSaveData> weaponAmmoStates = new SerializableDictionary<string, WeaponSaveData>();


        /// <summary>
        /// Saves the current magazine and total ammo for a specific weapon.
        /// </summary>
        /// <param name="weaponID">The ID of the weapon.</param>
        /// <param name="magazine">The current ammo in magazine.</param>
        /// <param name="total">The total reserve ammo.</param>
        public void SaveAmmoData(string weaponID, int magazine, int total)
        {
            weaponAmmoStates[weaponID] = new WeaponSaveData
            {
                weaponID = weaponID,
                magazineCurrent = magazine,
                totalAmmo = total
            };
        }


        /// <summary>
        /// Retrieves the saved ammo data for the given weapon ID.
        /// If no data exists, returns default values based on weapon type.
        /// </summary>
        /// <param name="weaponID">The ID of the weapon.</param>
        /// <returns>WeaponSaveData containing magazine and reserve ammo counts.</returns>
        public WeaponSaveData GetAmmoData(string weaponID)
        {
            if (weaponAmmoStates.TryGetValue(weaponID, out var data))
                return data;

            EquipmentDataSO weaponData = weaponDataBase.GetWeaponData(weaponID);
            if (weaponData.weaponType == WeaponType.Rifle)
            {
                return new WeaponSaveData
                {
                    weaponID = weaponID,
                    magazineCurrent = 20,
                    totalAmmo = 60
                };
            }
            else if (weaponData.weaponType == WeaponType.Grenade)
            {
                return new WeaponSaveData
                {
                    weaponID = weaponID,
                    magazineCurrent = 0,
                    totalAmmo = 5
                };
            }

            return new WeaponSaveData
            {
                weaponID = weaponID,
                magazineCurrent = 0,
                totalAmmo = 0
            };

        }


        /// <summary>
        /// Checks whether the specified weapon has bullets remaining in its magazine.
        /// </summary>
        /// <param name="weaponID">The ID of the weapon.</param>
        /// <returns>True if magazine has ammo; otherwise false.</returns>
        public bool HasAmmoInMagazine(string weaponID) =>
            weaponAmmoStates.TryGetValue(weaponID, out var data) && data.magazineCurrent > 0;


        /// <summary>
        /// Checks whether the specified weapon has reserve ammo available.
        /// </summary>
        /// <param name="weaponID">The ID of the weapon.</param>
        /// <returns>True if reserve ammo is greater than 0; otherwise false.</returns>
        public bool HasReserveAmmo(string weaponID) =>
            weaponAmmoStates.TryGetValue(weaponID, out var data) && data.totalAmmo > 0;
    }
}
