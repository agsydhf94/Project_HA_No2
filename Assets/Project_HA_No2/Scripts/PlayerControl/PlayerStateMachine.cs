using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerStateMachine
    {
        public PlayerState currentState { get; private set; }
        public PlayerState subState { get; private set; }   

        public void Initialize(PlayerState startState)
        {
            currentState = startState;
            currentState.EnterState();
            subState = null;
        }

        public void ChangeState(PlayerState newState)
        {
            currentState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }

        public void SubState_On(PlayerState state)
        {
            if(subState != null)
                subState.ExitState();

            subState = state;
            subState.EnterState();
        }

        public void SubState_Off()
        {
            subState.ExitState();
            subState = null;
        }
    }
}
