namespace HA
{
    public class PlayerCounterAttackState : PlayerState
    {
        private bool canCreateClone;

        public PlayerCounterAttackState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            canCreateClone = true;

            stateTimer = playerCharacter.counterAttackDuration;
            playerCharacter.characterAnimator.SetBool("SuccessfulCounterAttack", false);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            playerCharacter.Character_SetZeroVelocity();

            var stunnableEnemy = playerCharacter.GetStunnableEnemies();
            if (stunnableEnemy != null)
            {
                stateTimer = 10f;
                playerCharacter.characterAnimator.SetBool("SuccessfulCounterAttack", true);

                if(canCreateClone)
                {
                    canCreateClone = false;
                    playerCharacter.skillManager.cloneSkill.CreateCloneOnCounterAttack(stunnableEnemy.transform);
                }
                
            }

            if (stateTimer < 0 || triggerCalled)
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
