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
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if(Input.GetKeyDown(KeyCode.Space))
            {
                stateMachine.ChangeState(playerCharacter.jumpState);
            }


        }

        public override void ExitState()
        {
            base.ExitState();
        }
   
    }
}
