using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class Enemy : CharacterBase
    {
        public EnemyStateMachine stateMachine { get; private set; }

        #region Enemy Moving Information
        [Header("Enemy Moving Information")]
        public float patrolSpeed;
        public float chaseSpeed;
        public float idleTime;
        public float patrolTime;
        #endregion


        protected override void Awake()
        {
            base.Awake();
            stateMachine = new EnemyStateMachine();
        }

        protected override void Update()
        {
            base.Update();
            stateMachine.currentState.UpdateState();
        }
    }
}
