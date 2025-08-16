using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    /// <summary>
    /// UI controller for a single quest entry.
    /// Displays quest metadata and renders reward items under the rewards root.
    /// </summary>
    public class QuestPrefab : MonoBehaviour
    {
        /// <summary>
        /// Text element showing the quest title.
        /// </summary>
        public TMP_Text questName;

        /// <summary>
        /// Text element showing the NPC related to the quest.
        /// </summary>
        public TMP_Text questNPC;

        /// <summary>
        /// Button that toggles tracking for this quest in the UI.
        /// </summary>
        public Button trackingActivateButton;

        /// <summary>
        /// Whether this quest entry is currently active in the UI.
        /// </summary>
        public bool isActive;

        /// <summary>
        /// Whether this quest is currently being tracked.
        /// </summary>
        public bool isTracking;
        
        /// <summary>
        /// Parent object under which reward item prefabs are instantiated.
        /// </summary>
        public GameObject rewardsPrefabRoot;

        /// <summary>
        /// Prefab used to display a single reward item (name, icon, quantity).
        /// </summary>
        public QuestRewardPrefab questRewardPrefab;


        /// <summary>
        /// Initializes the quest UI using the provided quest data.
        /// Instantiates a <see cref="QuestRewardPrefab"/> for each reward
        /// found in <see cref="QuestData.questInfoSO"/> and populates it.
        /// </summary>
        /// <param name="questData">Quest data that includes reward items to render.</param>
        public void Initialize(QuestData questData)
        {
            foreach (KeyValuePair<ItemDataSO, int> pair in questData.questInfoSO.rewardItems)
            {
                var rewardPrefab = Instantiate(questRewardPrefab, rewardsPrefabRoot.transform);
                rewardPrefab.Initialize(pair.Key, pair.Value);
            }
        }
    }
}
