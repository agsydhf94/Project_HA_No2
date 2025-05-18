using UnityEngine;

namespace HA
{
    public class WeaponHandler : MonoBehaviour
    {
        private IWeapon currentWeapon;
        private GunViewModel gunViewModel = new GunViewModel();

        public void SetWeapon(IWeapon weapon, WeaponMetaData meta)
        {
            currentWeapon = weapon;

            gunViewModel.SetMeta(meta);

            if (weapon is RifleWeapon rifle)
                gunViewModel.SetBullet(rifle.TransferBulletData());
        }

        public GunViewModel GetViewModel() => gunViewModel;


        public void TriggerAttack()
        {
            currentWeapon?.Attack();
        }

        public void TriggerReload()
        {
            if (currentWeapon is IReloadeable reloadable)
                reloadable.Reload();
        }

        public void NotifyAmmoChanged(RifleWeapon rifle)
        {
            gunViewModel.SetBullet(rifle.TransferBulletData());
        }
    }
}

