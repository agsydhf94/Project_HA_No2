using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyBear : Enemy
    {
        #region States
        public EnemyIdleState IdleState { get; private set; }
        public EnemyPatrolState patrolState { get; private set; }
        #endregion

        public override void Awake()
        {
            base.Awake();

            IdleState = new EnemyIdleState(this, stateMachine, "Idle", this);
            patrolState = new EnemyPatrolState(this, stateMachine, "Patrol", this);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
