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
    }
}
