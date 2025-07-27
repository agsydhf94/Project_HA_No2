using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    /// <summary>
    /// Controls the visual health bar UI using image fill amounts.
    /// Responds to health changes from CharacterStats and updates both immediate and delayed visuals.
    /// </summary>
    public class HealthBarUI : MonoBehaviour
    {
        /// <summary>
        /// Reference to the CharacterStats component to monitor health changes.
        /// </summary>
        private CharacterStats characterStats;

        /// <summary>
        /// Image used to display the current (immediate) health value.
        /// </summary>
        [SerializeField] private Image foregroundImage;

        /// <summary>
        /// Component responsible for animating delayed fill feedback on health loss.
        /// </summary>
        [SerializeField] private DelayedFillController delayedFill;


        /// <summary>
        /// Subscribes to health change events and initializes the health bar display.
        /// </summary>
        private void Start()
        {
            characterStats = GetComponentInParent<CharacterStats>();
            characterStats.onHealthChanged += UpdateHealthUI;
            delayedFill = GetComponent<DelayedFillController>();
            UpdateHealthUI();
        }


        /// <summary>
        /// Updates the health bar fill visuals based on the character's current health.
        /// </summary>
        private void UpdateHealthUI()
        {
            float cur = characterStats.currentHp;
            float max = characterStats.GetMaxHealthValue();
            float normalized = cur / max;

            foregroundImage.fillAmount = normalized;
            delayedFill?.SetFillAmount(normalized);
        }


        /// <summary>
        /// Unsubscribes from the health change event when the UI is disabled.
        /// </summary>
        private void OnDisable()
        {
            characterStats.onHealthChanged -= UpdateHealthUI;
        }
    }
}
