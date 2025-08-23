
namespace HA
{
    /// <summary>
    /// Interface for weapons that use ammunition, extending from IWeapon.
    /// </summary>
    public interface IAmmoWeapon : IWeapon
    {
        /// <summary>
        /// Injects the current ammo status into the weapon (e.g., current/maximum ammo).
        /// </summary>
        /// <param name="status">The ammo status to apply to the weapon.</param>
        void InjectAmmoStatus(WeaponAmmoStatus status);

        /// <summary>
        /// Transfers bullet-related data (e.g., damage, speed, effects) for firing.
        /// </summary>
        /// <returns>A BulletData object containing bullet properties.</returns>
        BulletData TransferBulletData();
    }
}
