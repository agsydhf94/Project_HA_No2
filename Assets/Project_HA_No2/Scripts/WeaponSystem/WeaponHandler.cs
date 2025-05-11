using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public abstract class WeaponHandler : MonoBehaviour
    {
        protected WeaponDataSO weaponData;

        public virtual void SetWeapon(WeaponDataSO data)
        {
            weaponData = data;
        }

        public abstract void Use();
        public abstract void Reload();
    }
}

