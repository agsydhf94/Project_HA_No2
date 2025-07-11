using UnityEngine;

namespace HA
{
    public class PlayerArmedState : PlayerState
    {
        public PlayerArmedState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            playerCharacter.subWalkingSpeedDelta = playerCharacter.armed_WalkingDelta;
            playerCharacter.subRunningSpeedDelta = playerCharacter.armed_RunningDelta;
        }

        public override void UpdateState()
        {
            // Prevents re-triggering logic if already in the primary attack state
            if (stateMachine.currentState == playerCharacter.primaryAttackState) return;
            
            if (Input.GetKeyDown(KeyCode.Y))
            {
                playerCharacter.CharacterUnArmed();
            }

            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                stateMachine.ChangeState(playerCharacter.primaryAttackState);
            }            
            

        }

        public override void ExitState()
        {
            base.ExitState();
            playerCharacter.subWalkingSpeedDelta = 0f;
            playerCharacter.subRunningSpeedDelta = 0f;
        }
    }
}
