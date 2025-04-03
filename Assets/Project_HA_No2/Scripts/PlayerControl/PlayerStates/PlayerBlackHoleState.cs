using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerBlackHoleState : PlayerState
    {
        private float skillTime = 0.5f;
        private bool skillUsed;

        public PlayerBlackHoleState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            skillUsed = false;
            stateTimer = skillTime;
            playerCharacter.characterAnimator.applyRootMotion = true;
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            playerCharacter.Character_SetZeroVelocity();

            if(stateTimer < 0 && !skillUsed)
            {
                if (playerCharacter.skillManager.blackHoleSkill.CanUseSkill())
                {
                    skillUsed = true;
                }
                    
            }

            if(playerCharacter.skillManager.blackHoleSkill.BlackHoleFinished())
            {
                stateMachine.ChangeState(playerCharacter.idleState);
            }
        }
    }
}
