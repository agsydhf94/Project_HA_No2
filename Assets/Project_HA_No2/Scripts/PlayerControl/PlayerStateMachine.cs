using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerStateMachine
    {
        public PlayerState currentState { get; private set; }
        public PlayerState currentSubState { get; private set; }   

        public void Initialize(PlayerState startState)
        {
            currentState = startState;
            currentState.EnterState();
            currentSubState = null;
        }

        public void ChangeState(PlayerState newState)
        {
            currentState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }

        public void ChangeSubState(PlayerState newSubState)
        {
            currentSubState.ExitState();
            currentSubState = newSubState;
            currentSubState.EnterState();
        }

        public void SubState_On(PlayerState state)
        {
            if(currentSubState != null)
                currentSubState.ExitState();

            currentSubState = state;
            currentSubState.EnterState();
        }

        public void SubState_Off()
        {
            currentSubState.ExitState();
            currentSubState = null;
        }
    }
}
