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

            List<Collider> colliders = ObjectDetection.GetObjectsBy<PlayerCharacter>(enemyBear.eyeCheck, enemyBear.eyeCheckDistance, enemyBear.playerLayerMask);
            if(colliders.Count > 0) // 플레이어가 감지되면
            {
                // 플레이어 감지시, 추척 타이머 리셋
                stateTimer = enemyBear.chaseTime;

                if (enemyBear.Distance_byRaycast() < enemyBear.attackDistance)
                {
                    if(Enemy.CanAttack(enemyBear))
                        stateMachine.ChangeState(enemyBear.attackState);

                }
            }
            else if(stateTimer < 0)
            {
                // 플레이어가 감지되지 않는 순간부터
                // 위에서 초기화된 타이머가 줄어들기 시작하여
                // 타이머가 다 되면 추적상태에서 벗어남
                stateMachine.ChangeState(enemyBear.IdleState);
            }
            enemyBear.ChaseMode_BySector(colliders, enemyBear.viewAngle);

        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
