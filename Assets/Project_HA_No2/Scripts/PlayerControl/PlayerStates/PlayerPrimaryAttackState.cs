using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerPrimaryAttackState : PlayerArmedState
    {
        private int comboCounter;

        private float lastTimeAttacked;
        private float comboWindow = 2;

        public PlayerPrimaryAttackState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            if(comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
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

            if(triggerCalled)
            {
                stateMachine.ChangeState(playerCharacter.idleState);
            }
        }

        public override void ExitState()
        {
            base.ExitState();

            comboCounter++;
            lastTimeAttacked = Time.time;
            playerCharacter.characterAnimator.applyRootMotion = false;
            Debug.Log(lastTimeAttacked);
        } 
    }
}
