using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HA
{
    /// <summary>
    /// Connects a ProgressTicker to UI elements like a progress bar and percentage text.
    /// </summary>
    public class ProgressUI : MonoBehaviour
    {
        /// <summary>
        /// Reference to the ProgressTicker that drives the progress values.
        /// </summary>
        [Header("Dependencies")]
        [SerializeField] private ProgressTicker progressTicker;


        /// <summary>
        /// UI Image component used as a fillable progress bar.
        /// </summary>
        [Header("UI Elements")]
        [SerializeField] private Image fillImage;

        /// <summary>
        /// Text field displaying the percentage progress value.
        /// </summary>
        [SerializeField] private TextMeshProUGUI percentText;


        /// <summary>
        /// If true, displays the progress as a percentage.
        /// </summary>
        [Header("Options")]
        [SerializeField] private bool showPercentageText = true;


        /// <summary>
        /// Initializes the UI and subscribes to ProgressTicker events.
        /// </summary>
        private void Awake()
        {
            if (progressTicker == null)
            {
                Debug.LogError("[ProgressUI] ProgressTicker is not assigned.");
                enabled = false;
                return;
            }

            progressTicker.onProgressTick += UpdateUI;
            progressTicker.onComplete += HandleComplete;

            UpdateUI(0f);

            progressTicker.StartTicker(2.0f);
        }


        /// <summary>
        /// Unsubscribes from events when the object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            progressTicker.onProgressTick -= UpdateUI;
            progressTicker.onComplete -= HandleComplete;
        }


        /// <summary>
        /// Updates the progress bar and text display based on the current progress value.
        /// </summary>
        /// <param name="value">Normalized progress value (0 to 1).</param>
        private void UpdateUI(float value)
        {
            if (fillImage != null)
                fillImage.fillAmount = value;

            if (percentText != null && showPercentageText)
                percentText.text = $"{Mathf.RoundToInt(value * 100f)}%";
        }

        /// <summary>
        /// Called when progress completes. Placeholder for completion feedback.
        /// </summary>
        private void HandleComplete()
        {
            // ex: Success effect, sound, or popup
            Debug.Log("[ProgressUI] Progress complete.");
        }
    }
}
