using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace HA
{
    public class QuestManager : SingletonBase<QuestManager>
    {
        public List<QuestData> allActiveQuests;
        public List<QuestData> allCompletedQuests;
        public List<QuestData> allTrackingQuests;

        public QuestMenuUI questMenuUI;
        
        public override void Awake()
        {
            base.Awake();
            questMenuUI = CanvasUI.Instance.GetQuestMenuUI();
        }

        private void OnEnable()
        {
            Inventory.OnItemCollectedEvent += NotifyObjectiveProgress;
            EnemyBase.OnEnemyKillEvent += NotifyObjectiveProgress;
        }

        private void OnDisable()
        {
            Inventory.OnItemCollectedEvent -= NotifyObjectiveProgress;
            EnemyBase.OnEnemyKillEvent -= NotifyObjectiveProgress;
        }

        public void HandoverQuestCompleted(QuestData questData)
        {
            allActiveQuests.Remove(questData);
            allCompletedQuests.Add(questData);
            UnTrackQuest(questData);

            questMenuUI.RefreshQuestList();
        }

        public void AddActiveQuest(QuestData quest)
        {
            ActivateQuestObjectives(quest);
            allActiveQuests.Add(quest);
            TrackQuest(quest);

            questMenuUI.RefreshQuestList();
        }

        public void ActivateQuestObjectives(QuestData quest)
        {
            QuestInfoSO questInfoSO = quest.questInfoSO;

            if(questInfoSO.requiredEnemies.Count > 0)
            {
                EnemyKillObjective enemyKillObject = new EnemyKillObjective();
                enemyKillObject.Initialize(questInfoSO);

                quest.questObjectives.Add(enemyKillObject);
            }
        }

        #region Quest Progress Tracking
        public void TrackQuest(QuestData quest)
        {
            allTrackingQuests.Add(quest);

            questMenuUI.RefreshTrackingList();
        }

        public void UnTrackQuest(QuestData quest)
        {
            allTrackingQuests.Remove(quest);

            questMenuUI.RefreshTrackingList();
        }

        
        public void NotifyObjectiveProgress(string id)
        {
            foreach (var quest in allActiveQuests)
            {
                foreach (var objective in quest.questObjectives)
                {
                    objective.UpdateProgress(id);

                    if (objective.IsCompleted)
                    {
                        Debug.Log($"[Quest] '{quest.questInfoSO.questName}'의 목표 달성됨");
                    }
                }
            }

            questMenuUI.RefreshTrackingList();
        }
        #endregion
    }
}
