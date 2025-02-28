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

            playerCharacter.characterAnimator.SetBool("IsGroundDetected", playerCharacter.IsGroundedDetected());
            

            if (playerCharacter.IsGroundedDetected())
            {    
                stateMachine.ChangeState(playerCharacter.idleState);
            }
            playerCharacter.ApplyGravity();
            playerCharacter.CharacterJump();

        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
