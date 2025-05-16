using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerRifleArmedState : PlayerState
    {
        public PlayerRifleArmedState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            playerCharacter.characterAnimator.SetTrigger("RifleArmedTrigger");
            playerCharacter.SetAimInput();

            playerCharacter.subWalkingSpeedDelta = playerCharacter.rifleArmed_WalkingDelta;
            playerCharacter.subRunningSpeedDelta = playerCharacter.rifleArmed_RunningDelta;
        }
        public override void UpdateState()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                playerCharacter.CharacterUnArmed();
            }
        }

        public override void ExitState()
        {
            base.ExitState();

            playerCharacter.RemoveAimInput();
        }

    }
}
