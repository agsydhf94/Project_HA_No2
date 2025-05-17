using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public interface IWeapon
    {
        public void InitializeWeaponData(WeaponData data);
        public void Attack();
    }
}
