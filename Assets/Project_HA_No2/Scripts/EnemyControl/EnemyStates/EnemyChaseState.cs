using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyChaseState : EnemyState
    {
        public EnemyChaseState(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolname, EnemyBear enemyBear) : base(enemyBase, stateMachine, animationBoolname)
        {
            this.enemyBear = enemyBear;
        }

        public override void EnterState()
        {
            base.EnterState();
            enemyBear.SetNavMeshAgent_Go();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            List<Collider> colliders = enemyBear.ObjectDetection<PlayerCharacter>(enemyBear.eyeCheck, enemyBear.eyeCheckDistance, enemyBear.playerLayerMask);
            if(colliders.Count > 0) // �÷��̾ �����Ǹ�
            {
                if (enemyBear.Distance_byRaycast() < enemyBear.attackDistance)
                {
                    stateMachine.ChangeState(enemyBear.attackState);
                }
            }
            enemyBear.ChaseMode_BySector(colliders, enemyBear.viewAngle);

        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
