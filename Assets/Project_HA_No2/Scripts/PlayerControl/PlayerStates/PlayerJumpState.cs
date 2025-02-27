using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerJumpState : PlayerState
    {
        public PlayerJumpState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            playerCharacter.GravityCalculate();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            playerCharacter.CharacterJump();
            playerCharacter.ApplyGravity();
            playerCharacter.characterAnimator.SetBool("IsGroundDetected", playerCharacter.IsGroundedDetected());

            if (playerCharacter.verticalVelocity < 0)
            {
                stateMachine.ChangeState(playerCharacter.airState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
