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
        [SerializeField] protected int knockbackVibrato;    // 진동 횟수 (숫자가 클수록 더 세밀한 떨림)
        [SerializeField] protected float knockbackstrength;   // 흔들리는 강도 (위치 변화 범위)
        [SerializeField] protected float knockbackRandomness;    // 랜덤한 방향성
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


        public void ApplyDamageFrom(CharacterStats attackerStats)
        {
            Debug.Log(gameObject.name + "Damaged");
            attackerStats.DoDamage(characterStats);

            HitKnockback().Forget();
        }

        #region Object Detection
        public static List<Collider> ObjectDetection<T>(Transform center, float radius, LayerMask layerMask = default) where T : class
        {
            // 마지막 인수를 쓰지 않으면, 레이어 상관없이 탐색
            if (layerMask == default) layerMask = ~0;

            // 조건에 맞는 콜라이더가 몇 개인지 길이 예측이 안되기 때문에 배열대신 리스트로 선언
            List<Collider> result = new List<Collider>();
            Collider[] colliders = Physics.OverlapSphere(center.position, radius, layerMask);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.TryGetComponent<T>(out T _objectClass))
                {
                    result.Add(collider);
                }
            }

            return result;
        }
        #endregion

        #region Knockback
        protected virtual async UniTask HitKnockback()
        {
            isKnocked = true;

            // 현재 위치 저장
            Vector3 originalPosition = gameObject.transform.position;

            // DoTween 흔들림 효과 (ShakePosition)
            Tween shakeTween = gameObject.transform.DOShakePosition(knockbackDuration, knockbackstrength, knockbackVibrato, knockbackRandomness)
                .SetEase(Ease.InOutQuad); // 부드러운 시작 및 종료

            await shakeTween.AsyncWaitForCompletion(); // UniTask로 변환하여 완료될 때까지 대기

            // 원래 위치로 복귀 (오차 방지)
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
