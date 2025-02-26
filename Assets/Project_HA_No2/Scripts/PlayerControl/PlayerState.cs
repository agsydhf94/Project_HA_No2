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

        public PlayerState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName)
        {
            this.playerCharacter = playerCharacter;
            this.stateMachine = stateMachine;
            this.animationBoolName = animationBoolName;
        }

        public virtual void EnterState()
        {
            playerCharacter.characterAnimator.SetBool(animationBoolName, true);
        }

        public virtual void UpdateState()
        {
            playerCharacter.CameraRotation();
        }

        public virtual void ExitState()
        {
            playerCharacter.characterAnimator.SetBool(animationBoolName, false);
        }
    }
}
