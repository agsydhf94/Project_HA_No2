using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HA
{
    public class EnemyBear : Enemy
    {
        #region States
        public EnemyIdleState IdleState { get; private set; }
        public EnemyPatrolState patrolState { get; private set; }
        public EnemyChaseState chaseState { get; private set; }
        public EnemyAttackState attackState { get; private set; }
        public EnemyStunnedState stunnedState { get; private set; }
        #endregion

        

        protected override void Awake()
        {
            base.Awake();

            IdleState = new EnemyIdleState(this, stateMachine, "Idle", this);
            patrolState = new EnemyPatrolState(this, stateMachine, "Patrol", this);
            chaseState = new EnemyChaseState(this, stateMachine, "Chase", this);
            attackState = new EnemyAttackState(this, stateMachine, "Attack", this);
            stunnedState = new EnemyStunnedState(this, stateMachine, "Stunned", this);
        }
        protected override void Start()
        {
            base.Start();
            stateMachine.Initialized(IdleState);
        }

        protected override void Update()
        {
            base.Update();
        }

        public override bool CanBeStunned()
        {
            if(base.CanBeStunned())
            {
                stateMachine.ChangeState(stunnedState);
                return true;
            }

            return false;
        }


    }
}
