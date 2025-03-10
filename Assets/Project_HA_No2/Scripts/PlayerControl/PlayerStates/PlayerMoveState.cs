using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace HA
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(PlayerCharacter playerController, PlayerStateMachine stateMachine, string animationBoolName) : base(playerController, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if(InputSystem.Instance.Movement.magnitude == 0)
            {
                stateMachine.ChangeState(playerCharacter.idleState);
            }

            playerCharacter.CharacterMove(playerCharacter.inputSystem.Movement, playerCharacter.mainCamera.transform.eulerAngles.y);
            
            
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        
    }
}
