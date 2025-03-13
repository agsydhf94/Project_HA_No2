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
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if(triggerCalled)
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
