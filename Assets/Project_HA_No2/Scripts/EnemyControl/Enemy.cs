using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class Enemy : CharacterBase
    {
        public Animator enemyAnimator;
        public EnemyStateMachine stateMachine { get; private set; }

        #region Enemy Moving Information
        [Header("Enemy Moving Information")]
        public float moveSpeed;
        public float idleTime;
        #endregion


        public override void Awake()
        {
            stateMachine = new EnemyStateMachine();
        }

        public override void Update()
        {
            base.Update();
            stateMachine.currentState.UpdateState();
        }
    }
}
