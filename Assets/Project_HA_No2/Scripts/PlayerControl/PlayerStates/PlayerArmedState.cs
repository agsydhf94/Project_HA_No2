using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerArmedState : PlayerState
    {
        public PlayerArmedState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (Input.GetKeyDown(KeyCode.Y))
            {
                playerCharacter.CharacterUnArmed();
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}
