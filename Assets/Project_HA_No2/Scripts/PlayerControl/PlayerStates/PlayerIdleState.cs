using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(PlayerCharacter playerController, PlayerStateMachine stateMachine, string animationBoolName) : base(playerController, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if(InputSystem.Instance.Movement.magnitude != 0)
            {
                stateMachine.ChangeState(playerCharacter.moveState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        
    }
}
