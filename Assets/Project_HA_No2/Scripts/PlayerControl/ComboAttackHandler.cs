using UnityEngine;
using System;
namespace HA
{
    /// <summary>
    /// Represents the timing data for a single combo attack step.
    /// </summary>
    public struct ComboInfo
    {
        /// <summary>
        /// Total duration of the combo animation.
        /// </summary>
        public float animationDuration;

        /// <summary>
        /// Time after which the attack is considered to have hit.
        /// </summary>
        public float hitTiming;

        /// <summary>
        /// Time after which combo buffering is allowed.
        /// </summary>
        public float bufferStartTime;

        /// <summary>
        /// Time until which combo buffering is allowed.
        /// </summary>
        public float bufferEndTime;

        public ComboInfo(float animDuration, float hitTime, float bufferStart, float bufferEnd)
        {
            animationDuration = animDuration;
            hitTiming = hitTime;
            bufferStartTime = bufferStart;
            bufferEndTime = bufferEnd;
        }
    }
    

    /// <summary>
    /// Handles the logic of combo-based melee attack chains.
    /// Supports combo buffering, input timing, animation triggers, and combo transitions.
    /// </summary>
    public class ComboAttackHandler : MonoBehaviour
    {
        /// <summary>
        /// Index of the current combo in the combo chain.
        /// </summary>
        private int currentComboIndex;

        /// <summary>
        /// Timer tracking how long the current combo has been playing.
        /// </summary>
        private float attackTimer;

        /// <summary>
        /// Whether the attack has already hit during this combo stage.
        /// </summary>
        private bool hasAttacked;

        /// <summary>
        /// Whether a combo input has been buffered during the buffer window.
        /// </summary>
        private bool comboBuffered;

        /// <summary>
        /// Whether the system is currently within a valid combo buffer window.
        /// </summary>
        private bool canBuffer;

        /// <summary>
        /// Whether the player has input an attack and it is being buffered for a future combo.
        /// </summary>
        private bool inputBuffered = false;

        /// <summary>
        /// Timer tracking how long the input buffer remains valid.
        /// </summary>
        private float inputBufferTimer = 0f;

        /// <summary>
        /// The fixed duration for which a combo input remains buffered.
        /// </summary>
        private const float inputBufferDuration = 0.25f;

        /// <summary>
        /// Locks input temporarily during combo transition to avoid double registration.
        /// </summary>
        private bool isInputLocked = false;

        /// <summary>
        /// Array containing the timing data for each combo stage.
        /// </summary>
        private ComboInfo[] comboInfos;

        /// <summary>
        /// Callback triggered when the attack hit timing is reached.
        /// </summary>
        private Action onHit;

        /// <summary>
        /// Callback triggered when the combo animation should be played.
        /// Passes the combo index (1-based).
        /// </summary>
        private Action<int> onComboAnimation;

        /// <summary>
        /// Callback triggered when the combo ends (due to timeout or completion).
        /// </summary>
        private Action onComboEnd;


        /// <summary>
        /// Initializes the combo handler with combo steps and external animation callbacks.
        /// </summary>
        /// <param name="combos">Array of combo timing information.</param>
        /// <param name="onHit">Callback when hit timing is reached.</param>
        /// <param name="onComboAnimation">Callback to play specific combo animation (index + 1).</param>
        /// <param name="onComboEnd">Callback when combo chain finishes or times out.</param>
        public void Initialize(
            ComboInfo[] combos,
            Action onHit,
            Action<int> onComboAnimation,
            Action onComboEnd)
        {
            comboInfos = combos;
            this.onHit = onHit;
            this.onComboAnimation = onComboAnimation;
            this.onComboEnd = onComboEnd;
        }


        /// <summary>
        /// Starts the combo attack sequence from the first combo step.
        /// </summary>
        public void StartAttack()
        {
            currentComboIndex = 0;
            attackTimer = 0f;
            hasAttacked = false;
            comboBuffered = false;
            canBuffer = false;

            onComboAnimation?.Invoke(currentComboIndex + 1);
        }


        /// <summary>
        /// Handles combo update per frame including buffer logic, transition timing, and input detection.
        /// Should be called manually in the character's update loop.
        /// </summary>
        public void OnUpdate()
        {
            if (comboInfos == null || comboInfos.Length == 0) return;


            attackTimer += Time.deltaTime;
            var currentCombo = comboInfos[currentComboIndex];


            // Enable buffer window
            if (attackTimer >= currentCombo.bufferStartTime)
            {
                canBuffer = true;
            }

            // Register combo buffer input if within timing
            if (canBuffer && inputBuffered)
            {
                comboBuffered = true;
                Debug.Log("Combo buffered input registered (input + timing satisfied)");
                inputBuffered = false; // Consume once
            }

            // Early combo transition
            if (comboBuffered &&
                attackTimer >= currentCombo.hitTiming &&
                attackTimer <= currentCombo.bufferEndTime &&  
                currentComboIndex < comboInfos.Length - 1)
            {
                Debug.Log("Executing early combo transition");
                ComboTransition();
                return;
            }

            // Combo ends if not transitioned and animation finished
            if (attackTimer >= currentCombo.animationDuration)
            {
                Debug.Log("Combo finished");
                onComboEnd?.Invoke();
            }


            // Input buffer registration
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isInputLocked && !inputBuffered)
            {
                inputBuffered = true;
                inputBufferTimer = inputBufferDuration;
                Debug.Log("Input buffer registered");
            }

        }


        /// <summary>
        /// Transitions to the next combo stage and resets state.
        /// </summary>
        private void ComboTransition()
        {
            inputBuffered = false;
            isInputLocked = true;

            currentComboIndex++;
            attackTimer = 0f;
            hasAttacked = false;
            comboBuffered = false;
            canBuffer = false;

            Debug.Log($"Transitioned to next combo: {currentComboIndex + 1}");
            onComboAnimation?.Invoke(currentComboIndex + 1);

            Invoke(nameof(UnlockTransition), 0.05f);
        }


        /// <summary>
        /// Unlocks input after a short transition delay.
        /// </summary>
        private void UnlockTransition()
        {
            isInputLocked = false;
        }


        /// <summary>
        /// Enables the ability to buffer combo inputs during a valid window.
        /// </summary>
        public void EnableComboBuffer()
        {
            Debug.Log("버퍼 가능 시작");
            canBuffer = true;
        }


        /// <summary>
        /// Disables buffering of combo inputs.
        /// </summary>
        public void DisableComboBuffer()
        {
            Debug.Log("버퍼 가능 종료");
            canBuffer = false;
        }


        /// <summary>
        /// Resets combo progress and internal flags.
        /// </summary>
        public void ResetCombo()
        {
            currentComboIndex = 0;
            attackTimer = 0f;
            hasAttacked = false;
            comboBuffered = false;
            canBuffer = false;
        }

    }
}
