using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HA
{
    public class EnemyBear : Enemy
    {
        #region States
        public EnemyIdleState IdleState { get; private set; }
        public EnemyPatrolState patrolState { get; private set; }
        #endregion

        #region NavMesh Components
        private NavMeshAgent navMeshAgent;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            navMeshAgent = GetComponent<NavMeshAgent>();

            IdleState = new EnemyIdleState(this, stateMachine, "Idle", this);
            patrolState = new EnemyPatrolState(this, stateMachine, "Patrol", this);
        }
        protected override void Start()
        {
            base.Start();
            stateMachine.Initialized(IdleState);
        }

        protected override void Update()
        {
            base.Update();
        }

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

        public void SetNavMeshAgent_Stop() => navMeshAgent.isStopped = true;
    }
}
