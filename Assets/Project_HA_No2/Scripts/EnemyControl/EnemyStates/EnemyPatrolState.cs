using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyPatrolState : EnemyGroundedState
    {
        public EnemyPatrolState(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolname, EnemyBear enemyBear) : base(enemyBase, stateMachine, animationBoolname, enemyBear)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            stateTimer = enemyBear.patrolTime;
            enemyBear.EnemyPatrol_RandomDirection();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if(stateTimer < 0 || enemyBear.IsGroundedDetected())
            {
                stateMachine.ChangeState(enemyBear.IdleState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
