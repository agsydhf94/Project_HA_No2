using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerState
    {
        protected PlayerCharacter playerCharacter;
        protected PlayerStateMachine stateMachine;
        private string animationBoolName;


        protected float stateTimer;
        protected bool triggerCalled;


        public PlayerState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName)
        {
            this.playerCharacter = playerCharacter;
            this.stateMachine = stateMachine;
            this.animationBoolName = animationBoolName;
        }

        public virtual void EnterState()
        {
            playerCharacter.characterAnimator.SetBool(animationBoolName, true);
            triggerCalled = false;
        }

        public virtual void UpdateState()
        {
            playerCharacter.CameraRotation();

            playerCharacter.ApplyNaturalGravity();
            playerCharacter.characterAnimator.SetBool("IsGroundDetected", playerCharacter.IsGroundedDetected());

            stateTimer -= Time.deltaTime;
        }

        public virtual void ExitState()
        {
            playerCharacter.characterAnimator.SetBool(animationBoolName, false);
        }

        public virtual void AnimationFinishTrigger() => triggerCalled = true;
    }
}
