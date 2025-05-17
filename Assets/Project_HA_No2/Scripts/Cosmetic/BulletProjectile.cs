using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class BulletProjectile : MonoBehaviour
    {
        private Rigidbody rb;
        private IObjectReturn objectReturn;
        private CharacterStats playerStats;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }


        public void Initialize(IObjectReturn objectReturn, CharacterStats stats)
        {
            this.objectReturn = objectReturn;
            playerStats = stats;

            float projectileSpeed = 40f;
            rb.velocity = transform.forward * projectileSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamagable damagable))
            {
                // 추후 WeaponEffect 메서드가 이 부분을 대체할 것으로 보임
                // entityVFX.SwordHitVFX();

                var target = other.transform.GetComponent<EnemyStat>();
                if (target != null)
                    playerStats.DoDamage(target);

                objectReturn.Return("bulletTrajectoryStick", this);
            }

            objectReturn.Return("bulletTrajectoryStick", this);
        }
    }
}
