using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerRifleAimState : PlayerState
    {
        public PlayerRifleAimState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            playerCharacter.isAiming = true;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            playerCharacter.SetShootingPosition();
            Debug.Log("AimState Ω««‡¡ﬂ");

            if (Input.GetMouseButtonUp(1))
            {
                stateMachine.ChangeSubState(playerCharacter.rifleArmedState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();

            playerCharacter.isAiming = false;
        }

        
    }
}
