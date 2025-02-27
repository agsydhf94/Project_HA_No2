using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HA
{
    public class CharacterBase : MonoBehaviour, IDamagable
    {
        [Header("Animation")]
        public Animator characterAnimator;
        public CharacterController characterController;

        [Header("Moving Properties")]
        public float unitSpeed;
        public float movingSpeed;
        public float horizontal;
        public float vertical;
        public float runningBlend;
        public float targetRotation;
        public float rotationSpeed;

        public bool IsRun { get; set; } = false;

        public virtual void Awake()
        {
            characterAnimator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
        }

        public virtual void Update()
        {
            
        }


        public void ApplyDamage(float damage)
        {
            
        }
    }
}
