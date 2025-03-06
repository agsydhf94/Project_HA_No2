using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HA
{
    public class CharacterBase : MonoBehaviour, IDamagable
    {
        #region Animation Components
        [Header("Animation Components")]
        public Animator characterAnimator;       
        #endregion

        #region Moving Information
        [Header("Moving Information")]
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
        #endregion

        #region Collision Information
        [Header("Collision Information")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private LayerMask groundLayer;
        #endregion

        public bool IsRun { get; set; } = false;

        public virtual void Awake()
        {
            characterAnimator = GetComponent<Animator>();
            
        }

        public virtual void Update()
        {
            
        }


        public void ApplyDamage(float damage)
        {
            
        }

        #region Collision
        public bool IsGroundedDetected() => Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundCheck.position, groundCheckDistance);
        }
        #endregion
    }
}
