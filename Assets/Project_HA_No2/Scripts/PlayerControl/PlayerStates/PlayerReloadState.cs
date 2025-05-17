using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerReloadState : PlayerState
    {
        public PlayerReloadState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            Debug.Log("RELOADING");

            if(triggerCalled)
            {
                playerCharacter.weaponHandler.TriggerReload();
                stateMachine.ChangeSubState(playerCharacter.rifleArmedState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
            Debug.Log("Reload Complete");
        }        
    }
}
