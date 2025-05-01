using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerDashState : PlayerState
    {
        public PlayerDashState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            playerCharacter.skillManager.dashSkill.CloneAttack();

            stateTimer = playerCharacter.dashDuration;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            playerCharacter.CharacterDash();
            playerCharacter.DashTrailRenderer_On();

            if(stateTimer < 0)
            {
                playerCharacter.DashTrailRenderer_Off();
                stateMachine.ChangeState(playerCharacter.idleState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        } 
    }
}
