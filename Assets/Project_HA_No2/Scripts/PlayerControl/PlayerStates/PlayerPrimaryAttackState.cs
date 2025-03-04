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

            // Time.time >= lastTimeAttacked + comboWindow �� �κ���
            // ���� Animation event trigger�� �����ϰ� �ֱ⿡ ���ʿ�
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
                // ������ �޺������� Queued �Ǹ� �ȵ�
                if(comboCounter < 2)
                {
                    comboAttackQueued = true;
                }                
            }

            // ���⼭�� trigger�� �ش� �޺� ���� �ִϸ��̼��� ���� ������ ����
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
