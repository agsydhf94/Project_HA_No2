using UnityEngine;

namespace HA
{
    public class RifleWeapon : WeaponHandler, IWeapon, IReloadeable
    {
        private PlayerCharacter playerCharacter;
        private CharacterStats playerStats;

        [Header("Weapon Informations")]
        [SerializeField] public float fireRate;
        [SerializeField] public int magazine_Current;
        [SerializeField] public int magazine_Capacity;
        [SerializeField] public int totalAmmo;
        public GameObject firePosition;

        public ObjectManager objectManager;
        public IObjectSpawner objectSpawner;
        private float lastTimeShoot;

        private void Awake()
        {
            if(objectSpawner == null)
            {
                objectManager = ObjectManager.Instance;
                InitializeSpawner(objectManager);
            }            
        }

        public void InitializeSpawner(IObjectSpawner spawner)
        {
            this.objectSpawner = spawner;
        }

        public void InitializeWeaponData(WeaponData data)
        {
            fireRate = data._fireRate;
            magazine_Current = data._magazineCurrent;
            magazine_Capacity = data._magazineCapacity;

            if(playerCharacter == null)
            {
                playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
                playerStats = playerCharacter.GetComponent<CharacterStats>();
            }
                 

            if (firePosition == null)
                firePosition = GameObject.FindGameObjectWithTag("FirePosition");
        }


        public void Reload()
        {
            // 이미 탄창이 가득 차 있는 경우
            if (magazine_Current >= magazine_Capacity)
            {
                Debug.Log("탄창이 이미 가득 찼습니다.");
                return;
            }

            if (totalAmmo <= 0)
            {
                Debug.Log("예비 탄약이 부족하여 재장전할 수 없습니다.");
                return;
            }

            // 탄창에 필요한 탄 수
            int neededAmmo = magazine_Capacity - magazine_Current;

            // 예비 탄약이 충분한 경우
            if (totalAmmo >= neededAmmo)
            {
                totalAmmo -= neededAmmo;
                magazine_Current = magazine_Capacity;
            }
            // 예비 탄약이 부족한 경우
            else
            {
                magazine_Current += totalAmmo;
                totalAmmo = 0;
            }

            Debug.Log($"재장전 완료: 탄창 = {magazine_Current}/{magazine_Capacity}, 남은 탄약 = {totalAmmo}");
        }

        public void Attack()
        {
            if (Time.time > lastTimeShoot + fireRate)
            {
                if(magazine_Current <= 0)
                    return;

                magazine_Current--;
                
                // Set Direction
                Vector3 shootingDirection = (playerCharacter.mouseWorldPosition - firePosition.transform.position).normalized;

                // Shoot
                var projectile = objectSpawner.Spawn("bulletTrajectoryStick", firePosition.transform.position, Quaternion.LookRotation(shootingDirection, Vector3.up));
                projectile.transform.forward = firePosition.transform.forward;

                var bulletProjectile_Component = projectile.GetComponent<BulletProjectile>();
                bulletProjectile_Component.Initialize(objectManager, playerStats);

                // Reset Timer
                lastTimeShoot = Time.time;
            }
        }
    }
}
