using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerCounterAttackState : PlayerState
    {
        public PlayerCounterAttackState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            stateTimer = playerCharacter.counterAttackDuration;
            playerCharacter.characterAnimator.SetBool("SuccessfulCounterAttack", false);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            playerCharacter.Character_SetZeroVelocity();

            if(playerCharacter.CheckStunnableEnemies())
            {
                stateTimer = 10f;
                playerCharacter.characterAnimator.SetBool("SuccessfulCounterAttack", true);
            }

            if(stateTimer < 0 || triggerCalled)
            {
                stateMachine.ChangeState(playerCharacter.idleState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }    
    }
}
