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

        private float holdRifleTimer = 0.6f;
        private bool isReload;

        public override void EnterState()
        {
            base.EnterState();

            stateTimer = holdRifleTimer;
            isReload = false;

            playerCharacter.rigBuilder.layers[0].rig.weight = 1f;

            playerCharacter.characterAnimator.SetTrigger("RifleArmedTrigger");

            playerCharacter.SetAimInput();

            playerCharacter.subWalkingSpeedDelta = playerCharacter.rifleArmed_WalkingDelta;
            playerCharacter.subRunningSpeedDelta = playerCharacter.rifleArmed_RunningDelta;
        }
        public override void UpdateState()
        {
            playerCharacter.SetShootingPosition();


            if (Input.GetMouseButton(1))
            {
                playerCharacter.characterAnimator.SetBool("RifleAim", true);
                playerCharacter.isAiming = true;
                Debug.Log("Rifle Aiming");
            }

            if (Input.GetMouseButtonUp(1))
            {
                playerCharacter.characterAnimator.SetBool("RifleAim", false);
            }

            if (Input.GetMouseButton(0))
            {
                playerCharacter.weaponHandler.TriggerAttack();
                stateTimer = holdRifleTimer;
            }            

            if (Input.GetKeyDown(KeyCode.R))
            {
                isReload = true;                
                stateMachine.ChangeSubState(playerCharacter.reloadState);
            }
                

            if (Input.GetKeyDown(KeyCode.G))
            {
                playerCharacter.RemoveAimInput();
                playerCharacter.CharacterUnArmed();
            }
        }

        public override void ExitState()
        {
            if(isReload == false)
                base.ExitState();

            playerCharacter.rigBuilder.layers[0].rig.weight = 0f;
        }

    }
}
