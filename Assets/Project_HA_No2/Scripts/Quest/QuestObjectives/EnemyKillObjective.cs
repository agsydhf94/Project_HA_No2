using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HA
{
    [System.Serializable]
    public class EnemyKillProgress
    {
        public string enemyID;
        public int currentKills;
        public int targetKills;

        public bool isCompleted => currentKills >= targetKills;
    }
    public class EnemyKillObjective : IQuestObjective
    {

        public List<EnemyKillProgress> progressList = new();
        public bool IsCompleted => progressList.All(p => p.isCompleted);

        public void Initialize(QuestInfoSO questInfo)
        {
            progressList.Clear();

            foreach (var pair in questInfo.requiredEnemies)
            {
                progressList.Add(new EnemyKillProgress
                {
                    enemyID = pair.Key.enemyID,
                    targetKills = pair.Value,
                    currentKills = 0
                });
            }
        }

        // 외부에서 이 메서드를 호출하여 진행 상황을 반영
        public void UpdateProgress(string enemyID)
        {
            foreach (var progress in progressList)
            {
                if (progress.enemyID == enemyID)
                {
                    progress.currentKills++;
                    break;
                }
            }
        }

        public string GetProgressDescription()
        {
            var lines = progressList.Select(p =>
                $"{p.enemyID}: {p.currentKills} / {p.targetKills}");
            return string.Join("\n", lines);
        }
    }
}
