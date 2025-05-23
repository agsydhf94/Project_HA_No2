using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyState
    {
        #region Enemy Common Components
        protected EnemyStateMachine stateMachine;
        protected Enemy enemyBase;
        #endregion

        #region Enemy Classes
        protected EnemyBear enemyBear;
        #endregion

        private string animationBoolname;
        protected bool triggerCalled;
        protected float stateTimer;

        public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolname)
        {
            this.enemyBase = enemyBase;
            this.stateMachine = stateMachine;
            this.animationBoolname = animationBoolname;
        }

        public virtual void EnterState()
        {
            triggerCalled = false;
            enemyBase.characterAnimator.SetBool(animationBoolname, true);
        }

        public virtual void UpdateState()
        {
            stateTimer -= Time.deltaTime;
        }

        public virtual void ExitState()
        {
            enemyBase.characterAnimator.SetBool(animationBoolname, false);
        }


        public virtual void AnimationFinishTrigger()
        {
            triggerCalled = true;
        }
    }
}
