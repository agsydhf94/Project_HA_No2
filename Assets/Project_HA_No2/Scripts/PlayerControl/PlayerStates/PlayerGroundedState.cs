using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerGroundedState : PlayerState
    {
        public PlayerGroundedState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            playerCharacter.currentWalkingSpeedDelta = 0f;
            playerCharacter.currentRunningSpeedDelta = playerCharacter.unArmed_RunningDelta;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if(Input.GetKeyDown(KeyCode.Z) && playerCharacter.skillManager.blackHoleSkill.blackHoleUnlocked)
            {
                stateMachine.ChangeState(playerCharacter.blackHoleState);
            }

            if(Input.GetKeyDown(KeyCode.Mouse1) && playerCharacter.skillManager.ballThrowSkill.ballThrowUnlocked)
            {
                stateMachine.ChangeState(playerCharacter.aimBallState);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.ChangeState(playerCharacter.jumpState);
            }

            if(Input.GetKeyDown(KeyCode.U))
            {
                playerCharacter.CharacterArmed();
            }

            if(Input.GetKeyDown(KeyCode.H))
            {
                playerCharacter.CharacterRifleArmed();
            }

            if(Input.GetKeyDown(KeyCode.Q) && playerCharacter.skillManager.counterAttackSkill.counterAttackUnlocked)
            {
                stateMachine.ChangeState(playerCharacter.counterAttackState);
            }

            if(!playerCharacter.IsGroundedDetected())
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
