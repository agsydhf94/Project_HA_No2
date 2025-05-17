using UnityEngine;

namespace HA
{
    public class WeaponHandler : MonoBehaviour
    {
        private IWeapon currentWeapon;

        public void SetWeapon(IWeapon weapon)
        {
            currentWeapon = weapon;
        }

        public void TriggerAttack()
        {
            currentWeapon?.Attack();
        }

        public void TriggerReload()
        {
            if (currentWeapon is IReloadeable reloadable)
                reloadable.Reload();
        }
    }
}

