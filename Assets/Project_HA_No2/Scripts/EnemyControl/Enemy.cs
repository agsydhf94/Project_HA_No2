using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.GameCenter;

namespace HA
{
    public class Enemy : CharacterBase
    {
        public EnemyStateMachine stateMachine { get; private set; }

        #region LayerMask and Sight Information
        public LayerMask playerLayerMask;
        public float viewAngle;
        #endregion

        #region NavMesh Components
        private NavMeshAgent navMeshAgent;
        #endregion

        #region Enemy Moving Information
        [Header("Enemy Moving Information")]
        public float patrolSpeed;
        public float chaseSpeed;
        public float idleTime;
        public float patrolTime;
        #endregion

        #region Enemy Attack Information
        public float attackDistance;
        #endregion


        protected override void Awake()
        {
            base.Awake();
            stateMachine = new EnemyStateMachine();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        protected override void Update()
        {
            base.Update();
            stateMachine.currentState.UpdateState();       
        }

        #region Enemy Patrol
        public void EnemyPatrol_RandomDirection()
        {
            float patrolRadius = Random.Range(30f, 40f);

            // xz ��鿡�� ������ ������ �����ϴ� �κ�
            // ���������� ������ ������ ���� ��, y���� ����
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.y = 0;


            // transform.position�� Origin ���� ����� ��ġ����
            Vector3 targetDestination = transform.position + randomDirection * patrolRadius;

            navMeshAgent.isStopped = false;
            navMeshAgent.speed = patrolSpeed;
            navMeshAgent.SetDestination(targetDestination);
        }
        #endregion

        #region Nav Mesh Agent Status
        public void SetNavMeshAgent_Stop() => navMeshAgent.isStopped = true;
        public void SetNavMeshAgent_Go() => navMeshAgent.isStopped = false;
        #endregion

        #region Object Detection
        public virtual List<Collider> ObjectDetection<T>(Transform center, float radius, LayerMask layerMask) where T : class
        {
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

        #region Distance Caluate by Raycast

        public float Distance_byRaycast()
        {
            float distance;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity, playerLayerMask))
            {
                distance = hit.distance; // �Ÿ� �� ����
            }
            else
            {
                distance = Mathf.Infinity; // �浹���� ���� ��� ���Ѵ� �Ÿ� ��ȯ
            }  

            return distance;
        }

        #endregion

        #region Enemy Chase
        public void ChaseMode_BySector(List<Collider> colliders, float viewAngle)
        {
            foreach (var collider in colliders)
            {
                Vector3 directonToTarget = collider.transform.position - transform.position;
                Vector3 forwardDirection = transform.forward;
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (Vector3.Angle(directonToTarget, forwardDirection) < viewAngle * 0.5f)
                {
                    navMeshAgent.speed = chaseSpeed;
                    navMeshAgent.SetDestination(collider.transform.position);
                }
            }
        }
        #endregion

        public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + attackDistance * transform.forward);
        }
    }
}
