using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// UI controller that handles displaying a list of quest rewards.
    /// Dynamically generates reward UI elements and plays reveal animations.
    /// </summary>
    public class QuestRewardUI : MonoBehaviour
    {
        /// <summary>
        /// The container transform where reward items will be instantiated.
        /// </summary>
        [SerializeField] private Transform rewardContainer;

        /// <summary>
        /// Prefab used for individual reward UI elements.
        /// </summary>
        [SerializeField] private GameObject rewardItemPrefab;

        /// <summary>
        /// Typing effect for the reward title text.
        /// </summary>
        public TextTyper titleTextTyper;

        /// <summary>
        /// UI fade scaler used for animating the overall reward panel.
        /// </summary>
        public UIFadeScaler uIFadeScaler;


        /// <summary>
        /// Populates the UI with reward items and plays staggered fly-in animations.
        /// </summary>
        /// <param name="rewardItems">Dictionary of reward items and their amounts.</param>
        public async UniTask SetRewardsAsync(SerializableDictionary<ItemDataSO, int> rewardItems)
        {
            foreach (Transform child in rewardContainer)
                Destroy(child.gameObject);

            var createdUIs = new List<QuestRewardItemUI>();

            foreach (var pair in rewardItems)
            {
                GameObject go = Instantiate(rewardItemPrefab, rewardContainer);
                var ui = go.GetComponent<QuestRewardItemUI>();
                ui.Set(pair.Key, pair.Value);
                createdUIs.Add(ui);
            }

            await UniTask.Yield(); // Ensure layout is updated before animation

            for (int i = 0; i < createdUIs.Count; i++)
            {
                await createdUIs[i].flyEffect.PlayWithDelayAsync(i * 0.1f);
                createdUIs[i].uIFadeScaler.PlayShow();
            }
        }
    }
}
