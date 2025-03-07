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
        }

        public override void UpdateState()
        {
            base.UpdateState();

            List<Collider> colliders = enemyBear.ObjectDetection<PlayerCharacter>(enemyBear.eyeCheck, enemyBear.eyeCheckDistance, enemyBear.playerLayerMask);
            if(colliders.Count > 0) // 플레이어가 감지되면
            {
                if (enemyBear.Distance_byRaycast() < enemyBear.attackDistance)
                {
                    Debug.Log("ATTACK");
                    enemyBear.SetNavMeshAgent_Stop();
                    return;
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
