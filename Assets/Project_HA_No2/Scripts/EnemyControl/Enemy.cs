using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class Enemy : MonoBehaviour
    {
        public Animator enemyAnimator;

        public EnemyStateMachine stateMachine { get; private set; }

        private void Awake()
        {
            stateMachine = new EnemyStateMachine();
        }

        private void Update()
        {
            stateMachine.currentState.UpdateState();
        }
    }
}
