using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HA
{
    public class ThrowBallSkillController : MonoBehaviour
    {
        private new Rigidbody rigidbody;
        private SphereCollider sphereCollider;
        private PlayerCharacter playerCharacter;
        private bool hasHitTarget = false;

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
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

        private void OnCollisionEnter(Collision collision)
        {
            hasHitTarget = true;
        }

    }
}
