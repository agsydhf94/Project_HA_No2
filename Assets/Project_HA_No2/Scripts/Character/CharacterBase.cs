using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HA
{
    public class CharacterBase : MonoBehaviour, IDamagable
    {
        #region Components
        [Header("Components")]
        public Animator characterAnimator;
        public EntityFX entityFx;
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
                    
        }

        protected virtual void Start()
        {
            characterAnimator = GetComponent<Animator>();
            entityFx = GetComponent<EntityFX>();
        }

        protected virtual void Update()
        {
            
        }


        public void ApplyDamage()
        {
            Debug.Log(gameObject.name + "Damaged");
        }

        #region Object Detection
        public static List<Collider> ObjectDetection<T>(Transform center, float radius, LayerMask layerMask = default) where T : class
        {
            // ������ �μ��� ���� ������, ���̾� ������� Ž��
            if (layerMask == default) layerMask = ~0;

            // ���ǿ� �´� �ݶ��̴��� �� ������ ���� ������ �ȵǱ� ������ �迭��� ����Ʈ�� ����
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
