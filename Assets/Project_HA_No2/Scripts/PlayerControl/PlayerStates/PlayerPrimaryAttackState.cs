using UnityEngine;

namespace HA
{
    /// <summary>
    /// Represents the player's primary melee attack state, handling combo logic,
    /// animation triggering, and state transitions.
    /// </summary>
    public class PlayerPrimaryAttackState : PlayerArmedState
    {
        /// <summary>
        /// Constructs the primary attack state and initializes the combo attack handler with predefined combos.
        /// </summary>
        /// <param name="playerCharacter">The player character associated with this state.</param>
        /// <param name="stateMachine">The player's state machine for transitions.</param>
        /// <param name="animationBoolName">The animation flag used to activate armed animations.</param>
        public PlayerPrimaryAttackState(PlayerCharacter playerCharacter, PlayerStateMachine stateMachine, string animationBoolName) : base(playerCharacter, stateMachine, animationBoolName)
        {
            // Define combo steps with animation timing, hit timing, and buffer windows
            var combos = new ComboInfo[]
            {
                new ComboInfo(1.111f, 0.283f, 0.05f, 0.9f),
                new ComboInfo(1.356f, 0.533f, 0.05f, 1.1f),
                new ComboInfo(1.6f, 0.667f, 0.05f, 1.3f)
            };

            // Initialize the combo attack handler with timing data and callbacks
            playerCharacter.comboAttackHandler.Initialize(
            combos,
            OnHit,
            OnComboAnimation,
            OnComboEnd
            );
        }
        
        /// <summary>
        /// Called when the player enters the primary attack state.
        /// Starts the combo attack sequence.
        /// </summary>
        public override void EnterState()
        {
            base.EnterState();
            playerCharacter.comboAttackHandler.StartAttack();
        }


        /// <summary>
        /// Called every frame while in this state. Updates combo logic.
        /// </summary>
        public override void UpdateState()
        {
            base.UpdateState();
            playerCharacter.comboAttackHandler.OnUpdate();
        }

        /// <summary>
        /// Called when exiting this state. Resets combo state.
        /// </summary>
        public override void ExitState()
        {
            base.ExitState();
            playerCharacter.comboAttackHandler.ResetCombo();
        }
        

        /// <summary>
        /// Callback triggered when the hit frame of the attack is reached.
        /// Executes the actual attack logic (e.g., damage).
        /// </summary>
        private void OnHit()
        {
            playerCharacter.weaponHandler.TriggerAttack();
        }


        /// <summary>
        /// Callback triggered when a combo animation stage begins.
        /// Sets animation parameters based on combo index.
        /// </summary>
        /// <param name="comboIndex">Current combo stage (1-based).</param>
        private void OnComboAnimation(int comboIndex)
        {
            // Set animator parameters to trigger the correct combo animation
            playerCharacter.characterAnimator.SetInteger("ComboCounter", comboIndex);
            playerCharacter.characterAnimator.SetTrigger("ComboTrigger");
            Debug.Log($"Combo {comboIndex} started!");
        }


        /// <summary>
        /// Callback triggered when the combo ends (due to timeout or completion).
        /// Transitions the player back to idle state.
        /// </summary>
        private void OnComboEnd()
        {
            playerCharacter.characterAnimator.SetTrigger("ComboEnd");
            Debug.Log("Combo ended → transitioning to Idle");
            stateMachine.ChangeState(playerCharacter.idleState);
        }
    }
}
