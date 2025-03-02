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
        public float basicSpeed;
        public float unArmed_RunningDelta;
        public float armed_WalkingDelta;
        public float armed_RunningDelta;
        [HideInInspector] public float unitSpeed;
        [HideInInspector] public float horizontal;
        [HideInInspector] public float vertical;
        [HideInInspector] public float runningBlend;
        [HideInInspector] public float targetRotation;
        [HideInInspector] public float rotationSpeed;

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
