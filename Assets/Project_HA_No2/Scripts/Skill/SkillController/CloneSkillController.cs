using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class CloneSkillController : MonoBehaviour
    {
        [SerializeField] private float cloneDuration;
        private float cloneTimer;

        private Animator animator;

        [SerializeField] private Transform attackCheck;
        [SerializeField] private float attackCheckRadius;
        private Transform closestEnemy;

        

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            cloneTimer -= Time.deltaTime;
        }

        

        public void SetUpClone(Transform newTransform, bool canAttack)
        {
            if (canAttack)
                animator.SetInteger("AttackNumber", Random.Range(1, 3));

            transform.position = newTransform.position;
            cloneTimer = cloneDuration;
        }

        private void AnimationTrigger_On()
        {
            cloneTimer = -0.1f;
        }

        private void AttackTrigger()
        {
            List<Collider> colliders = CharacterBase.ObjectDetection<IDamagable>(attackCheck, attackCheckRadius);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out IDamagable damagable))
                {
                    damagable.ApplyDamage();
                }
            }
        }

        private void FaceClosestTarget()
        {
            List<Collider> coliiders = CharacterBase.ObjectDetection<IDamagable>(transform, 25f);

            float closestDistance = Mathf.Infinity;
            
            foreach(var collider in coliiders)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);

                if (distanceToEnemy < closestDistance)
                    closestEnemy = collider.transform;
            }

            if(closestEnemy != null)
            {
                // To do : Face the clone towards the closest enemy
            }
        }
    }
}
