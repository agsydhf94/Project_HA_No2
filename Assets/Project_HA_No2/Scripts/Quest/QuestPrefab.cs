using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class QuestPrefab : MonoBehaviour
    {
        public TMP_Text questName;
        public TMP_Text questNPC;

        public Button trackingActivateButton;

        public bool isActive;
        public bool isTracking;

        public GameObject rewardsPrefabRoot;
        public QuestRewardPrefab questRewardPrefab;
        SerializableDictionary<ItemDataSO, int> rewards;

        private void Awake()
        {
            if(rewards != null)
            {
                foreach (KeyValuePair<ItemDataSO, int> pair in rewards)
                {
                    var rewardPrefab = Instantiate(questRewardPrefab, rewardsPrefabRoot.transform);
                    rewardPrefab.Initialize(pair.Key, pair.Value);
                }
            }            
        }
    }
}
