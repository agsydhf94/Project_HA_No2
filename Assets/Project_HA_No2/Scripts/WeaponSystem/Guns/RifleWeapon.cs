using System;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{    
    /// <summary>
    /// Represents a firearm weapon with magazine-based ammo management, raycast-based shooting,
    /// and VFX/SFX integration. Implements IAmmoWeapon and IReloadeable to support modular weapon systems.
    /// </summary>
    public class RifleWeapon : MonoBehaviour, IAmmoWeapon, IReloadeable
    {
        private PlayerCharacter playerCharacter;
        private CharacterStats playerStats;
        private SoundManager soundManager;
        private AudioSource audioSource;
        private CameraSystem cameraSystem;

        /// <summary>
        /// List of recoil patterns applied sequentially on each shot.
        /// </summary>
        public List<Vector3> recoilShakePattern = new List<Vector3>();

        /// <summary>
        /// Tracks the current index in the recoil pattern list.
        /// </summary>
        public int currentRecoilIndex = 0;

        [Header("Weapon Informations")]
        private float fireRate;             // Time between shots
        private float projectileForce;      // Optional: used for physics-based projectiles
        private int magazine_Current;       // Current ammo in magazine
        private int magazine_Capacity;      // Magazine size
        private int totalAmmo;              // Reserve ammo count
        public GameObject firePosition;     // Origin point of raycast or bullet spawn
        private string muzzleFlashKey;      // VFX key for muzzle flash
        private string bulletImpactKey;     // VFX key for bullet impact

        private VFXManager vfxManager;
        private float lastTimeShoot;

        private string fireSoundId;
        private string equipSoundId;
        private string reloadSoundId;

        private WeaponAmmoStatus ammoStatus;        // External ammo save/load system reference
        private string weaponID;                    // Unique ID for ammo tracking

        [Header("Raycast Settings")]
        public LayerMask hitLayer;                  // Layer mask for valid raycast targets
        public float raycastDistance = 100f;        // Max raycast range

        private void Start()
        {
            if (vfxManager == null)
            {
                vfxManager = VFXManager.Instance;
            }

            soundManager = SoundManager.Instance;
        }


        /// <summary>
        /// Initializes this weapon with data from EquipmentDataSO.
        /// Sets sound, VFX keys, and player component references.
        /// </summary>
        public void InitializeWeaponData(EquipmentDataSO _equipmentData)
        {
            fireRate = _equipmentData.fireRate;
            projectileForce = _equipmentData.projectileForce;
            magazine_Capacity = _equipmentData.magazine_Capacity;

            fireSoundId = _equipmentData.fireSoundID;
            equipSoundId = _equipmentData.equipSoundID;
            muzzleFlashKey = _equipmentData.muzzleFlashKey;
            bulletImpactKey = _equipmentData.bulletImpactKey;

            weaponID = _equipmentData.itemID;

            if (playerCharacter == null)
            {
                playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
                playerStats = playerCharacter.GetComponent<CharacterStats>();
            }

            if (cameraSystem == null)
            {
                cameraSystem = CameraSystem.Instance;
            }

            firePosition = GameObject.FindGameObjectWithTag("FirePosition");

            PlayEquipSound();
        }


        /// <summary>
        /// Injects ammo data from external save system and updates internal values.
        /// </summary>
        public void InjectAmmoStatus(WeaponAmmoStatus ammoStatus)
        {
            this.ammoStatus = ammoStatus;
            WeaponSaveData ammoData = ammoStatus.GetAmmoData(weaponID);
            magazine_Current = ammoData.magazineCurrent;
            totalAmmo = ammoData.totalAmmo;
        }


        /// <summary>
        /// Reloads the weapon from reserve ammo, adjusting internal counts and notifying listeners.
        /// </summary>
        public void Reload()
        {
            if (magazine_Current >= magazine_Capacity)
                return;

            if (totalAmmo <= 0)
                return;

            int neededAmmo = magazine_Capacity - magazine_Current;

            if (totalAmmo >= neededAmmo)
            {
                totalAmmo -= neededAmmo;
                magazine_Current = magazine_Capacity;
            }
            else
            {
                magazine_Current += totalAmmo;
                totalAmmo = 0;
            }

            playerCharacter.weaponHandler.NotifyAmmoChanged(this);
            ammoStatus.SaveAmmoData(weaponID, magazine_Current, totalAmmo);
        }


        /// <summary>
        /// Fires a raycast-based shot, applies VFX/SFX, and triggers damage if an enemy is hit.
        /// </summary>
        public void Attack()
        {
            if (Time.time > lastTimeShoot + fireRate)
            {
                if (magazine_Current <= 0)
                    return;

                magazine_Current--;
                soundManager.PlaySound(fireSoundId, transform.position);
                cameraSystem.ShakeCamera(GetShakeVector(), 0.2f, 1f);

                vfxManager.PlayEffect(muzzleFlashKey, firePosition.transform.position, Quaternion.LookRotation(firePosition.transform.forward), null, 0.9f, VFXSourceType.ObjectPool);

                Vector3 shootingDirection = (playerCharacter.mouseWorldPosition - firePosition.transform.position).normalized;

                Ray ray = new Ray(firePosition.transform.position, shootingDirection);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastDistance, hitLayer))
                {
                    if (hitInfo.transform.TryGetComponent(out EnemyStat target))
                    {
                        playerStats.DoDamage(target); // 예시 데미지 처리
                    }

                    vfxManager.PlayEffect(bulletImpactKey, hitInfo.point, Quaternion.LookRotation(hitInfo.normal), null, 0.7f, VFXSourceType.ObjectPool);
                }

                lastTimeShoot = Time.time;

                playerCharacter.weaponHandler.NotifyAmmoChanged(this);
                ammoStatus.SaveAmmoData(weaponID, magazine_Current, totalAmmo);
            }
        }


        /// <summary>
        /// Retrieves the current recoil pattern vector and updates the index.
        /// </summary>
        public Vector3 GetShakeVector()
        {
            Vector3 velocity = recoilShakePattern[currentRecoilIndex];
            currentRecoilIndex++;
            if (currentRecoilIndex >= recoilShakePattern.Count)
                currentRecoilIndex = 0;

            return velocity;
        }


        /// <summary>
        /// Plays the equip sound effect for this weapon.
        /// </summary>
        public void PlayEquipSound()
        {
            soundManager.PlaySound(equipSoundId, transform.position);
        }


        /// <summary>
        /// Transfers current ammo state to be saved externally.
        /// </summary>
        public BulletData TransferBulletData()
        {
            return new BulletData
            {
                _magazineCurrent = magazine_Current,
                _totalAmmo = totalAmmo
            };
        }
    }
}
