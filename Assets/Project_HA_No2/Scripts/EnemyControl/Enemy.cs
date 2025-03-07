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

            // xz 평면에서 랜덤한 방향을 설정하는 부분
            // 단위구에서 랜덤한 방향을 설정 후, y성분 제거
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.y = 0;


            // transform.position을 Origin 으로 고려한 위치벡터
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
        public virtual List<Collider> IsObjectDetected<T>(Transform center, float radius, LayerMask layerMask) where T : class
        {
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

        #region Enemy Chase
        public void ChaseMode_BySector(List<Collider> colliders, float viewAngle)
        {
            foreach (var collider in colliders)
            {
                Vector3 directonToTarget = collider.transform.position - transform.position;
                Vector3 forwardDirection = transform.forward;

                if (Vector3.Angle(directonToTarget, forwardDirection) < viewAngle * 0.5f)
                {
                    navMeshAgent.speed = chaseSpeed;
                    navMeshAgent.SetDestination(collider.transform.position);
                }
            }
        }
        #endregion
    }
}
