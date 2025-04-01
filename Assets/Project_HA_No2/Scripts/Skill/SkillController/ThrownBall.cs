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

        public LayerMask enemyLayer;
        public VFXManager vfxManager;

        public int crashCount = 0;
        private Transform currentTarget;

        private IObjectReturn objectReturn;
        public event Action OnReturnRequested;

        private void Awake()
        {
            vfxManager = VFXManager.Instance;
            rigidbody = GetComponent<Rigidbody>();
        }

        public async void Initialize(IObjectReturn returnHandler)
        {
            this.objectReturn = returnHandler;
            crashCount = 0;

            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            rigidbody.isKinematic = true; // 초기화는 끄고, 던질 때 다시 켜기
        }


        private void OnTriggerEnter(Collider other)
        {

            if (((1 << other.gameObject.layer) & enemyLayer) != 0)
            {
                crashCount++;

                // 일반 VFX
                vfxManager.PlayEffect("mari_BallSkillHit", transform.position, Quaternion.identity, null, 0.5f);

                Debug.Log($"[{crashCount}] 충돌 대상 : {other.gameObject.name}");

                if (crashCount >= 5)
                {
                    // 마지막 피니시 VFX
                    vfxManager.PlayEffect("mari_BallSkillHit_Final", transform.position, Quaternion.identity, null, 0.8f);
                    currentTarget = null;
                    Return();
                }
                else
                {
                    Transform nextTarget = FindNextTarget(other.transform);
                    if (nextTarget != null)
                    {
                        StartChaining(nextTarget).Forget();
                    }
                    else
                    {
                        Return(); // 더 이상 타겟 없음
                        currentTarget = null;
                    }
                }
            }
        }

        #region Chain Attack

        public async UniTask ChainBallAttack(Transform[] targets, float speed)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                Transform target = targets[i];

                while (Vector3.Distance(transform.position, target.position) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                    await UniTask.Yield();
                }

            }

            objectReturn.Return("skillBall", this);; // 마지막 타격 후 공 제거
        }

        public async UniTask StartChaining(Transform target)
        {
            currentTarget = target;

            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, 20f * Time.deltaTime);
                await UniTask.Yield();
            }

            // 위치에 도달해도 충돌이 없을 경우 대비
            await UniTask.Delay(TimeSpan.FromMilliseconds(200));
            if (crashCount < 5)
            {
                Return();
            }
        }

        private Transform FindNextTarget(Transform from)
        {
            float minDistance = float.MaxValue;
            Transform closest = null;

            Collider[] hits = Physics.OverlapSphere(from.position, 10f, enemyLayer);
            foreach (var hit in hits)
            {
                if (hit.transform == from) continue;

                float dist = Vector3.Distance(from.position, hit.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = hit.transform;
                }
            }

            return closest;
        }
        #endregion

        private void Return()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.isKinematic = true;

            objectReturn.Return("skillBall", this);
        }



    }
}
