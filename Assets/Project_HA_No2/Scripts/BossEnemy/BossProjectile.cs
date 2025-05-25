using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace HA
{
    public class BossProjectile : MonoBehaviour
    {
        private IObjectReturn objectReturn;
        
        [Header("Projectile Properties")]
        public float damagePoint;
        public CharacterStats enemyCharacterStat;
        [SerializeField] private Vector3 attackDirection;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private Rigidbody rb;

        public float timer;
        public float projectile_TimeLimit;
        public bool impactFlag;
        [SerializeField] private LayerMask otherLayerMask;
        [SerializeField] private LayerMask playerLayerMask;



        private void Start()
        {
            impactFlag = false;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if(timer > projectile_TimeLimit && !impactFlag)
            {
                ReturnToPool().Forget();
            }

        }

        
        private void FixedUpdate()
        {
            Vector3 newPos = rb.position + attackDirection * projectileSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
        

        public void Initialize(IObjectReturn _objectReturn, CharacterStats characterStat)
        {
            objectReturn = _objectReturn;
            enemyCharacterStat = characterStat;
            impactFlag = false;
            timer = 0f;
        }

        public void SetTargetDirection(Vector3 direction, float projectileSpeed)
        {
            attackDirection = direction.normalized;
            this.projectileSpeed = projectileSpeed;

            rb.velocity = attackDirection * projectileSpeed;

            transform.rotation = Quaternion.LookRotation(attackDirection);
        }

        public async UniTask ReturnToPool()
        {
            await UniTask.WaitUntil(() => impactFlag);
            await UniTask.Delay(500);

            objectReturn.Return("boss_ProjectileA", this);
        }


        private void OnTriggerEnter(Collider collision)
        {
            if (impactFlag) return;

            Vector3 hitPosition = collision.transform.position;

            if ((otherLayerMask & (1 << collision.gameObject.layer)) != 0)
            {
                Debug.Log($"충돌한 오브젝트 레이어: {collision.gameObject.layer}");
                impactFlag = true;

                VFXManager.Instance.PlayEffect("bossProjectileExplosionPrefab", hitPosition, Quaternion.identity, null, 2f);
                ReturnToPool().Forget();
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log($"충돌한 오브젝트 레이어: {collision.gameObject.layer}");
                impactFlag = true;

                if (collision.transform.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    var target = collision.transform.GetComponent<PlayerStat>();
                    if (target != null)
                        enemyCharacterStat.DoDamage(target);

                    VFXManager.Instance.PlayEffect("bossProjectileExplosionPrefab", hitPosition, Quaternion.identity, null, 2f);
                    ReturnToPool().Forget();
                }
            }

            
        }


        private async UniTask ImpactPrefab_Return(ParticleSystem impactPrefab)
        {
            await UniTask.Delay(2000);
            ObjectPool.Instance.ReturnToPool("BossSegmentExplosionPrefab", impactPrefab);
        }
    }
}
