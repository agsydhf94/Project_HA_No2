using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    /// <summary>
    /// UI prefab controller for displaying and updating a quest objective's progress.
    /// Binds to a <see cref="QuestObjectiveViewModel"/> to receive change notifications,
    /// updates text and completion checkboxes accordingly, and triggers a blinking
    /// visual effect when the objective is completed.
    /// </summary>
    public class QuestObjectivePrefab : MonoBehaviour
    {
        /// <summary>
        /// UI text element showing the objective's progress (e.g., "2 / 5").
        /// </summary>
        [SerializeField] private TMP_Text progressText;

        /// <summary>
        /// Image representing the incomplete checkbox state.
        /// </summary>
        [SerializeField] private Image checkBoxNo;

        /// <summary>
        /// Image representing the completed checkbox state.
        /// </summary>
        [SerializeField] private Image checkBoxCompleted;

        /// <summary>
        /// ImageBlinker component used to trigger a blinking effect when the objective is completed.
        /// </summary>
        public ImageBlinker imageBlinker;

        /// <summary>
        /// The color used for the blinking effect on completion.
        /// </summary>
        public Color blinkingColor;

        /// <summary>
        /// The bound ViewModel providing quest objective data and change notifications.
        /// </summary>
        private QuestObjectiveViewModel viewModel;


        /// <summary>
        /// Initializes the prefab with the given quest objective data.
        /// This method sets the initial progress text without binding for updates.
        /// </summary>
        /// <param name="questObjective">The quest objective providing progress description.</param>
        public void Initialize(IQuestObjective questObjective)
        {
            progressText.text = questObjective.GetProgressDescription();
        }


        /// <summary>
        /// Binds the prefab to a <see cref="QuestObjectiveViewModel"/> and subscribes to change notifications.
        /// Triggers an initial UI refresh after binding.
        /// </summary>
        /// <param name="vm">The ViewModel to bind to.</param>
        public void Bind(QuestObjectiveViewModel vm)
        {
            viewModel = vm;
            viewModel.OnChanged += Refresh;

            Refresh();
        }


        /// <summary>
        /// Updates the UI elements to reflect the current objective progress and completion state.
        /// If the objective is completed, triggers the blinking completion effect.
        /// </summary>
        private void Refresh()
        {
            progressText.text = viewModel.ProgressText;
            checkBoxNo.gameObject.SetActive(!viewModel.IsCompleted);
            checkBoxCompleted.gameObject.SetActive(viewModel.IsCompleted);

            if (viewModel.IsCompleted)
            {
                float blinkingDuration = 1f;
                imageBlinker.BlinkForSeconds(blinkingDuration, blinkingColor, blinkingColor).Forget();
            }
        }
    }
}
