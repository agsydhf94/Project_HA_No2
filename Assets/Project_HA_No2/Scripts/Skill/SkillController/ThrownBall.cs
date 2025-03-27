using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using System;

namespace HA
{
    public class ThrownBall : MonoBehaviour
    {
        private new Rigidbody rigidbody;
        private bool hasHitTarget = false;

        public LayerMask enemyLayer;
        public VFXManager vfxManager;

        private void Awake()
        {
            vfxManager = VFXManager.Instance;
            rigidbody = GetComponent<Rigidbody>();
        }

        public void SetUpBall(Vector3 initialVelocity)
        {
            rigidbody.isKinematic = false;
            rigidbody.velocity = initialVelocity;
        }

        public async UniTask ChainBallAttack(Transform[] targets, float speed)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                Transform target = targets[i];

                while (!hasHitTarget && Vector3.Distance(transform.position, target.position) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                    await UniTask.Yield();
                }

                hasHitTarget = false;
            }

            Destroy(gameObject); // 마지막 타격 후 공 제거
        }


        private void OnTriggerEnter(Collider other)
        {
            if ((enemyLayer.value & (1 << other.gameObject.layer)) != 0)
            {
                hasHitTarget = true;

                BallExplosion();
                Debug.Log("지금 충돌 :" + other.gameObject.name);
            }
        }

        private void BallExplosion()
        {
            Vector3 fxPosition = transform.position;
            Quaternion fxRotation = Quaternion.identity; // 필요 시 방향 지정
            vfxManager.PlayEffect("mari_BallSkillHit", fxPosition, fxRotation, null, 0.5f);
        }

        
    }
}
