using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            playerCharacter.Character_SetZeroVelocity();
        }
    }
}
