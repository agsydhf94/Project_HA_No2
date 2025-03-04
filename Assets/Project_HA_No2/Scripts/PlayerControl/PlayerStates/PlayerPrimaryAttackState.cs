using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerPrimaryAttackState : PlayerArmedState
    {
        private int comboCounter;

        public PlayerPrimaryAttackState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            // Time.time >= lastTimeAttacked + comboWindow 이 부분은
            // 현재 Animation event trigger로 통제하고 있기에 불필요
            if (comboCounter > 2)
            {
                comboCounter = 0;
            }
            Debug.Log(comboCounter);
            playerCharacter.characterAnimator.applyRootMotion = true;
            playerCharacter.characterAnimator.SetInteger("ComboCounter", comboCounter);
        }

        public override void UpdateState()
        {
            base.UpdateState();        

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                // 마지막 콤보에서는 Queued 되면 안됨
                if(comboCounter < 2)
                {
                    comboAttackQueued = true;
                }                
            }

            // 여기서의 trigger는 해당 콤보 공격 애니메이션이 거의 끝나는 시점
            if(triggerCalled)
            {
                if (comboAttackQueued && comboCounter < 3)
                {
                    comboAttackQueued = false;
                    comboCounter++;
                    stateMachine.ChangeState(playerCharacter.primaryAttackState);
                }
                else
                {
                    comboCounter = 0;
                    stateMachine.ChangeState(playerCharacter.idleState);
                }
            }
        }

        public override void ExitState()
        {
            base.ExitState();

            playerCharacter.characterAnimator.applyRootMotion = false;
        } 
    }
}
