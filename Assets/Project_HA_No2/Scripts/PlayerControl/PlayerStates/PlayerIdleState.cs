using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(PlayerCharacter playerController, PlayerStateMachine stateMachine, string animationBoolName) : base(playerController, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            #region UnArmed - Idle => Move
            if (InputSystem.Instance.Movement.magnitude != 0)
            {
                stateMachine.ChangeState(playerCharacter.moveState);
            }
            #endregion

            #region Armed - Idle => Move
            if(Input.GetKeyDown(KeyCode.U))
            {

            }
            #endregion

        }

        public override void ExitState()
        {
            base.ExitState();
        }

        
    }
}
