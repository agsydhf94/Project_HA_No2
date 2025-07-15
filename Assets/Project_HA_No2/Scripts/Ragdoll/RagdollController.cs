using UnityEngine;
using UnityEngine.AI;

namespace HA
{
    /// <summary>
    /// Manages ragdoll activation and force-based ragdoll effects on a humanoid character.
    /// Automatically collects child rigidbodies and colliders, and handles transitions between animated and physics-driven states.
    /// </summary>
    public class RagdollController : MonoBehaviour
    {
        public Rigidbody[] rigidbodies;
        public Collider[] colliders;
        public Animator animator;
        public NavMeshAgent navMeshAgent;

        [Header("Ragdoll Foece Active")]
        public HumanBodyBones targetBone;
        public Vector3 forceDirection;
        public float forcePower;

        private void Awake()
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            colliders = GetComponentsInChildren<Collider>();
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            // Disable physics initially
            RidigBodyActive(false);
            ColliderActive(true);
        }

        /// <summary>
        /// Debug method to activate ragdoll and apply force to the configured bone from the Inspector.
        /// </summary>
        [ContextMenu("Active Ragdoll With Power")]
        public void ActiveForceRagdollWithPower()
        {
            ForceActiveRagdollWithPower(targetBone, forceDirection, forcePower);
        }


        /// <summary>
        /// Enables ragdoll and applies force to a specific bone.
        /// </summary>
        /// <param name="bone">The bone to apply the force to.</param>
        /// <param name="direction">Direction of the applied force.</param>
        /// <param name="power">Magnitude of the force.</param>
        public void ForceActiveRagdollWithPower(HumanBodyBones bone, Vector3 direction, float power)
        {
            SetRagdollActive(true);

            var boneTransform = animator.GetBoneTransform(bone);            // Animator�� ���ؼ�, HumanBodyBones�� Transform�� ������
            var targetRigidbody = boneTransform.GetComponent<Rigidbody>();  // ������ Bone Transform���� Rigidbody ������
            targetRigidbody.AddForce(direction * power, ForceMode.Force);   // ������ Rigidbody�� ���� ���Ѵ�
        }

        /// <summary>
        /// Disables animation and AI navigation, then enables ragdoll.
        /// </summary>
        [ContextMenu("Active Ragdoll")]
        public void ActiveRagdoll()
        {
            // to do : Ragdoll�� Ȱ��ȭ ��ų���� Animator, NavMesh ���� ��� ���ִ� ���� ����
            animator.enabled = false;
            navMeshAgent.enabled = false;
            SetRagdollActive(true);
        }


        /// <summary>
        /// Toggles the ragdoll state by enabling/disabling rigidbodies and colliders.
        /// </summary>
        /// <param name="isActive">Whether the ragdoll should be active.</param>
        public void SetRagdollActive(bool isActive)
        {
            RidigBodyActive(isActive);

            ColliderActive(isActive);
        }


        /// <summary>
        /// Enables or disables all child rigidbodies' physics simulation.
        /// </summary>
        /// <param name="isActive">If true, enables dynamic physics; otherwise sets rigidbodies to kinematic.</param>
        public void RidigBodyActive(bool isActive)
        {
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = !isActive;
            }
        }


        /// <summary>
        /// Enables or disables all child colliders.
        /// </summary>
        /// <param name="isActive">If true, enables colliders; otherwise disables them.</param>
        public void ColliderActive(bool isActive)
        {
            foreach (var col in colliders)
            {
                col.enabled = isActive;
            }
        }
    }
}
