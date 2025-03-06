using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class EnemyStateMachine
    {
        public EnemyState currentState { get; private set; }

        public void Initialized(EnemyState startState)
        {
            currentState = startState;
            currentState.EnterState();
        }

        public void ChangeState(EnemyState newState)
        {
            currentState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }
    }
}
