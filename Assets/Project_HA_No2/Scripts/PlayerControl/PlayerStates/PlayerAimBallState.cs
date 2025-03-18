using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerAimBallState : PlayerState
    {
        public PlayerAimBallState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
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

            if(Input.GetKeyUp(KeyCode.Mouse1))
            {
                stateMachine.ChangeState(playerCharacter.idleState);
            }
        }
    }
}
