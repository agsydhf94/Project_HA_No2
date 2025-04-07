using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyDeadState : EnemyState
    {
        private EnemyBear enemyBear;

        public EnemyDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolname, EnemyBear enemyBear) : base(enemyBase, stateMachine, animationBoolname)
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
        }
    }
}
