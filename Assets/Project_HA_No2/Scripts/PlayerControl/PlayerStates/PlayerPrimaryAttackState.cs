using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class PlayerPrimaryAttackState : PlayerArmedState
    {
        public PlayerPrimaryAttackState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void UpdateState()
        {
            base.UpdateState();        
        }

        public override void ExitState()
        {
            base.ExitState();
        } 
    }
}
