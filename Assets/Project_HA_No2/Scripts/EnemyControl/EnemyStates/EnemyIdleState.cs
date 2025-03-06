using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyIdleState : EnemyState
    {
        private EnemyBear enemyBear;

        public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animationBoolname, EnemyBear enemyBear) : base(enemy, stateMachine, animationBoolname)
        {
            this.enemyBear = enemyBear;
        }

        public override void EnterState()
        {
            base.EnterState();

            stateTimer = enemyBear.idleTime;
            enemyBear.SetNavMeshAgent_Stop();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            // ���� �ð����� ���ִٰ� ���� ���·� ��ȯ
            if (stateTimer < 0)
            {
                stateMachine.ChangeState(enemyBear.patrolState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }  
    }
}

