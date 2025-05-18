using System;
using UnityEngine;

namespace HA
{

    

    public class GunViewModel
    {        
        // Meta Data
        public string WeaponName { get; private set; }
        public Sprite Icon { get; private set; }
        public WeaponType WeaponType { get; private set; }

        // Bullet Data
        public int CurrentAmmo { get; private set; }
        public int TotalAmmo { get; private set; }


        public event Action OnDataChanged;

        public void SetMeta(WeaponMetaData meta)
        {
            WeaponName = meta._weaponName;
            Icon = meta._weaponIcon;
            WeaponType = meta._weaponType;
            
            OnDataChanged?.Invoke();
        }

        public void SetBullet(BulletData bullet)
        {
            CurrentAmmo = bullet._magazineCurrent;
            TotalAmmo = bullet._totalAmmo;
            OnDataChanged?.Invoke();
        }
    }
}
