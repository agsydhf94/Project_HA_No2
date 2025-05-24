using UnityEngine;
using UnityEngine.AI;

namespace HA
{
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

            // SetRagdollActive(false);
            RidigBodyActive(false);
            ColliderActive(true);
        }

        [ContextMenu("Active Ragdoll With Power")]
        public void ActiveForceRagdollWithPower()       // �����Ϳ��� �ӽ÷� Ȯ�ο� menu �Լ�
        {
            // public�� ���� �� �ӽ� �������� Ȱ���Ͽ� Ragdoll�� Force�� �༭ Ȱ��ȭ ��Ų��
            ForceActiveRagdollWithPower(targetBone, forceDirection, forcePower);
        }

        public void ForceActiveRagdollWithPower(HumanBodyBones bone, Vector3 direction, float power)
        {
            SetRagdollActive(true);

            var boneTransform = animator.GetBoneTransform(bone);            // Animator�� ���ؼ�, HumanBodyBones�� Transform�� ������
            var targetRigidbody = boneTransform.GetComponent<Rigidbody>();  // ������ Bone Transform���� Rigidbody ������
            targetRigidbody.AddForce(direction * power, ForceMode.Force);   // ������ Rigidbody�� ���� ���Ѵ�
        }

        // ����� ��
        [ContextMenu("Active Ragdoll")]
        public void ActiveRagdoll()
        {
            // to do : Ragdoll�� Ȱ��ȭ ��ų���� Animator, NavMesh ���� ��� ���ִ� ���� ����
            animator.enabled = false;
            navMeshAgent.enabled = false;
            SetRagdollActive(true);
        }

        public void SetRagdollActive(bool isActive)
        {
            RidigBodyActive(isActive);

            ColliderActive(isActive);
        }

        public void RidigBodyActive(bool isActive)
        {
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = !isActive;
            }
        }

        public void ColliderActive(bool isActive)
        {
            foreach (var col in colliders)
            {
                col.enabled = isActive;
            }
        }
    }
}
