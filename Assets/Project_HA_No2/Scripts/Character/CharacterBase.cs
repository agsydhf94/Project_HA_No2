using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace HA
{
    public class CharacterBase : MonoBehaviour, IDamagable
    {

        #region Components
        [Header("Components")]
        public Animator characterAnimator;
        public CharacterStats characterStats { get; private set; }
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

        #region Knockback Information
        [Header("Knockback Information")]
        [SerializeField] protected float knockbackDuration;
        [SerializeField] protected int knockbackVibrato;    // ���� Ƚ�� (���ڰ� Ŭ���� �� ������ ����)
        [SerializeField] protected float knockbackstrength;   // ��鸮�� ���� (��ġ ��ȭ ����)
        [SerializeField] protected float knockbackRandomness;    // ������ ���⼺
        protected bool isKnocked;
        #endregion

        #region Collision Information
        [Header("Collision Information")]
        public Transform attackCheck;
        public float attackCheckRadius;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckDistance;
        [SerializeField] public Transform eyeCheck;
        [SerializeField] public float eyeCheckDistance;
        [SerializeField] public LayerMask groundLayer;
        [SerializeField] public LayerMask defaultLayer;
        #endregion

        public bool IsRun { get; set; } = false;

        protected virtual void Awake()
        {
            characterStats = GetComponent<CharacterStats>();
        }

        protected virtual void Start()
        {
            
            characterAnimator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            
        }

        public virtual void GetSlowBy(float percentage, float slowDuration)
        {
            
        }

        protected virtual void ReturnDefaultSpeed()
        {
            characterAnimator.speed = 1;
        }

        #region Life Related
        public void DamageImpact() => HitKnockback().Forget();

        public virtual void Die()
        {

        }       
        #endregion

        

        #region Knockback
        protected virtual async UniTask HitKnockback()
        {
            isKnocked = true;

            // ���� ��ġ ����
            Vector3 originalPosition = gameObject.transform.position;

            // DoTween ��鸲 ȿ�� (ShakePosition)
            Tween shakeTween = gameObject.transform.DOShakePosition(knockbackDuration, knockbackstrength, knockbackVibrato, knockbackRandomness)
                .SetEase(Ease.InOutQuad); // �ε巯�� ���� �� ����

            await shakeTween.AsyncWaitForCompletion(); // UniTask�� ��ȯ�Ͽ� �Ϸ�� ������ ���

            // ���� ��ġ�� ���� (���� ����)
            gameObject.transform.position = originalPosition;

            isKnocked = false;
        }
        #endregion

        #region Collision
        public bool IsGroundedDetected() => Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundCheck.position, groundCheckDistance);
            Gizmos.DrawSphere(attackCheck.position, attackCheckRadius);
        }
        #endregion

    }
}
