using TMPro;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Controls the UI prefab for displaying a quest's name and its list of objectives.
    /// Dynamically instantiates <see cref="QuestObjectivePrefab"/> instances for each objective
    /// and binds them to their corresponding <see cref="QuestObjectiveViewModel"/>.
    /// </summary>
    public class QuestTrackingPrefab : MonoBehaviour
    {
        /// <summary>
        /// Text element displaying the quest's name.
        /// </summary>
        [SerializeField] private TMP_Text questNameText;

        /// <summary>
        /// Root GameObject that contains all instantiated objective prefabs.
        /// </summary>
        [SerializeField] private GameObject objectiveRoot;

        /// <summary>
        /// Prefab used to display individual quest objectives.
        /// </summary>
        [SerializeField] private QuestObjectivePrefab questTrackingObjectivesPrefab;

        /// <summary>
        /// ImageBlinker component for visual feedback, e.g., highlighting quest updates.
        /// </summary>
        [SerializeField] private ImageBlinker imageBlinker;


        /// <summary>
        /// Initializes the tracking UI with quest information and its objectives.
        /// Instantiates objective prefabs, binds them to their view models, and sets initial UI state.
        /// </summary>
        /// <param name="quest">Quest data containing info and objectives to display.</param>
        public void Initialize(QuestData quest)
        {
            questNameText.text = quest.questInfoSO.questName;

            // Example: Could trigger quest name blinking when initialized
            // imageBlinker.BlinkForSeconds(1f).Forget();

            foreach (IQuestObjective questObjective in quest.questObjectives)
            {
                var objectivePrefab = Instantiate(questTrackingObjectivesPrefab, objectiveRoot.transform);

                // Create and initialize the ViewModel for the objective
                var viewModel = objectivePrefab.gameObject.AddComponent<QuestObjectiveViewModel>();
                viewModel.Initialize(questObjective);

                // Bind EnemyKillObjective to its ViewModel for progress updates
                if (questObjective is EnemyKillObjective enemyKillObjective)
                    enemyKillObjective.BindViewModel(viewModel);

                // Initialize and bind the prefab component to the ViewModel
                var objectivePrefabComponent = objectivePrefab.GetComponent<QuestObjectivePrefab>();
                objectivePrefabComponent.Initialize(questObjective);
                objectivePrefabComponent.Bind(viewModel);
            }
        }
    }
}
