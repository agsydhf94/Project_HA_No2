using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyGroundedState : EnemyState
    {
        public EnemyGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolname, EnemyBear enemyBear) : base(enemyBase, stateMachine, animationBoolname)
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

            if(enemyBear.ObjectDetection<PlayerCharacter>(enemyBear.eyeCheck, enemyBear.eyeCheckDistance, enemyBear.playerLayerMask).Count > 0)
            {
                stateMachine.ChangeState(enemyBear.chaseState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }      
    }
}
