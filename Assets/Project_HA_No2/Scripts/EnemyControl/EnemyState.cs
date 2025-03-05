using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyState : MonoBehaviour
    {
        protected EnemyStateMachine stateMachine;
        protected Enemy enemy;

        private string animationBoolname;
        protected bool triggerCalled;
        protected float stateTimer;

        public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animationBoolname)
        {
            this.enemy = enemy;
            this.stateMachine = stateMachine;
            this.animationBoolname = animationBoolname;
        }

        public virtual void EnterState()
        {
            triggerCalled = false;
            enemy.enemyAnimator.SetBool(animationBoolname, true);
        }

        public virtual void UpdateState()
        {

        }

        public virtual void ExitState()
        {
            enemy.enemyAnimator.SetBool(animationBoolname, false);
        }
    }
}
