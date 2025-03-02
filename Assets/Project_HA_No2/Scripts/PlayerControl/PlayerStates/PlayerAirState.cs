using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerAirState : PlayerState
    {
        public PlayerAirState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();        
        }

        public override void UpdateState()
        {
            base.UpdateState();
            
            if (playerCharacter.IsGroundedDetected())
            {
                stateMachine.ChangeState(playerCharacter.idleState);
                playerCharacter.playerMovementVec = Vector3.zero;
            }
            playerCharacter.ApplyModifiedGravity();
            playerCharacter.CharacterJump();
            playerCharacter.characterAnimator.SetFloat("stopDetectGroundDuration", stateTimer);

        }

        public override void ExitState()
        {
            base.ExitState();            
        }
    }
}
