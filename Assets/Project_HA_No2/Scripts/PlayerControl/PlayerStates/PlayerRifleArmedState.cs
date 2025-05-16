using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            playerCharacter.characterAnimator.SetBool("StillRifleArmed", true);

            playerCharacter.SetAimInput();

            playerCharacter.subWalkingSpeedDelta = playerCharacter.rifleArmed_WalkingDelta;
            playerCharacter.subRunningSpeedDelta = playerCharacter.rifleArmed_RunningDelta;
        }
        public override void UpdateState()
        {
            playerCharacter.SetShootingPosition();
            Debug.Log("ArmedState Ω««‡¡ﬂ");

            if(Input.GetMouseButtonDown(1))
            {
                stateMachine.ChangeSubState(playerCharacter.rifleAimState);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                playerCharacter.characterAnimator.SetBool("StillRifleArmed", false);
                playerCharacter.RemoveAimInput();
                playerCharacter.CharacterUnArmed();
            }
        }

        public override void ExitState()
        {
            base.ExitState();     
        }

    }
}
