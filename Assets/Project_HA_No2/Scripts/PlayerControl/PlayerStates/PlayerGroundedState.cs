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

            if(Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.ChangeState(playerCharacter.jumpState);
            }

            // ���� ���ִ� ���¿��� ���� ���
            if(Input.GetKeyDown(KeyCode.C))
            {
                stateMachine.ChangeState(playerCharacter.dashState);
            }

            if(Input.GetKeyDown(KeyCode.U))
            {
                playerCharacter.CharacterArmed();
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }
   
    }
}
