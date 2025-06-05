using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HA
{
    [System.Serializable]
    public class ItemCollectionProgress
    {
        public string itemID;
        public int currentCount;
        public int requiredCount;

        public bool IsCompleted => currentCount >= requiredCount;
    }

    public class ItemCollectObjective : IQuestObjective
    {
        private List<ItemCollectionProgress> progressList = new();

        public bool IsCompleted => progressList.All(p => p.IsCompleted);

        public void Initialize(QuestInfoSO questInfo)
        {
            progressList.Clear();

            foreach (var pair in questInfo.requiredItems)
            {
                progressList.Add(new ItemCollectionProgress
                {
                    itemID = pair.Key.itemID,
                    requiredCount = pair.Value,
                    currentCount = 0
                });
            }
        }

        public void UpdateProgress(string itemID)
        {
            var progress = progressList.FirstOrDefault(p => p.itemID == itemID);
            if (progress != null && !progress.IsCompleted)
            {
                progress.currentCount++;
            }
        }

        public string GetProgressDescription()
        {
            var builder = new System.Text.StringBuilder();
            foreach (var progress in progressList)
            {
                builder.AppendLine($"- {progress.itemID}: {progress.currentCount}/{progress.requiredCount}");
            }
            return builder.ToString();
        }
    }
}
