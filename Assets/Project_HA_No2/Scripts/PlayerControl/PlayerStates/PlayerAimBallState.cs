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

            playerCharacter.Character_SetZeroVelocity();
            playerCharacter.detectTargetOnScreen.UpdateTargetList();

            if(Input.GetKeyUp(KeyCode.Mouse1))
            {
                Transform target = playerCharacter.detectTargetOnScreen.GetCurrentTarget()?.GetTargetPoint();
                playerCharacter.skillManager.ballThrowSkill.createdBall.transform.parent = null;
                playerCharacter.skillManager.ballThrowSkill.ThrowBallWithTarget(playerCharacter.skillManager.ballThrowSkill.createdBall ,target);

                stateMachine.ChangeState(playerCharacter.idleState);
            }
        }
    }
}
