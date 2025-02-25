using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerState : MonoBehaviour
    {
        protected PlayerController playerController;
        protected PlayerStateMachine stateMachine;
        private string animationBoolName;

        public PlayerState(PlayerController playerController, PlayerStateMachine stateMachine, string animationBoolName)
        {
            this.playerController = playerController;
            this.stateMachine = stateMachine;
            this.animationBoolName = animationBoolName;
        }

        public virtual void EnterState()
        {

        }

        public virtual void UpdateState()
        {

        }

        public virtual void ExitState()
        {

        }
    }
}
