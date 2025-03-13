using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyStunnedState : EnemyState
    {
        public EnemyStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolname, EnemyBear enemyBear) : base(enemyBase, stateMachine, animationBoolname)
        {
            this.enemyBear = enemyBear;
        }

        public override void EnterState()
        {
            base.EnterState();

            stateTimer = enemyBear.stunnedDuration;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if(stateTimer < 0)
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
