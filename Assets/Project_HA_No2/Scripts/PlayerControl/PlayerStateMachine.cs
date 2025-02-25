using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerStateMachine
    {
        public PlayerState currentState { get; private set; }

        public void Initialize(PlayerState startState)
        {
            currentState = startState;
            currentState.EnterState();
        }

        public void ChangeState(PlayerState newState)
        {
            currentState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }
    }
}
