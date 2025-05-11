using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] private Transform weaponSocket;
        private WeaponHandler currentHandler;

        public void EquipWeapon(WeaponDataSO data)
        {
            if (currentHandler != null)
                Destroy(currentHandler.gameObject);

            GameObject weaponGO = Instantiate(data.prefab, weaponSocket);
            weaponGO.transform.localPosition = Vector3.zero;
            weaponGO.transform.localRotation = Quaternion.identity;

            currentHandler = weaponGO.GetComponent<WeaponHandler>();
            currentHandler?.SetWeapon(data);
        }

        public void UseWeapon() => currentHandler?.Use();
        public void ReloadWeapon() => currentHandler?.Reload();
    }
}
