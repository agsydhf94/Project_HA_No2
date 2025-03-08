using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolname, EnemyBear enemyBear) : base(enemyBase, stateMachine, animationBoolname)
        {
            this.enemyBear = enemyBear;
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            // 공격 시에는 정지
            enemyBear.SetNavMeshAgent_Stop();

            if(triggerCalled)
            {
                stateMachine.ChangeState(enemyBear.chaseState);
                return;
            }

        }

        public override void ExitState()
        {
            base.ExitState();

            enemyBear.lastTimeAttacked = Time.time;
        }   
    }
}
