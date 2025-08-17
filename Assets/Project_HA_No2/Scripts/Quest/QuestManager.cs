using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Central manager responsible for handling quest state, objectives,
    /// and progress tracking. Supports activation, tracking, completion,
    /// and event-driven updates via the EventBus.
    /// </summary>
    public class QuestManager : SingletonBase<QuestManager>
    {
        /// <summary>
        /// List of all currently active quests.
        /// </summary>
        public List<QuestData> allActiveQuests;

        /// <summary>
        /// List of quests that have been completed.
        /// </summary>
        public List<QuestData> allCompletedQuests;

        /// <summary>
        /// List of quests currently being tracked in the UI.
        /// </summary>
        public List<QuestData> allTrackingQuests;

        /// <summary>
        /// Reference to the quest menu UI instance.
        /// </summary>
        public QuestMenuUI questMenuUI;


        /// <summary>
        /// Initializes the singleton and retrieves the QuestMenuUI.
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            questMenuUI = CanvasUI.Instance.GetQuestMenuUI();
        }


        /// <summary>
        /// Subscribes to relevant quest progress events on enable.
        /// </summary>
        private void OnEnable()
        {
            EventBus.Instance.Subscribe<GameEvent.EnemyKilled>(OnEnemyKilled);
            EventBus.Instance.Subscribe<GameEvent.ItemCollected>(OnItemCollected);
        }


        /// <summary>
        /// Unsubscribes from quest progress events on disable.
        /// </summary>
        private void OnDisable()
        {
            EventBus.Instance.Unsubscribe<GameEvent.EnemyKilled>(OnEnemyKilled);
            EventBus.Instance.Unsubscribe<GameEvent.ItemCollected>(OnItemCollected);
        }


        /// <summary>
        /// Moves a quest from active to completed, untracks it,
        /// and refreshes the quest menu UI.
        /// </summary>
        /// <param name="questData">The quest to mark as completed.</param>
        public void HandoverQuestCompleted(QuestData questData)
        {
            allActiveQuests.Remove(questData);
            allCompletedQuests.Add(questData);
            UnTrackQuest(questData);
            questMenuUI.RefreshQuestList();
        }


        /// <summary>
        /// Adds a quest to the active quests, initializes its objectives,
        /// starts tracking it, and refreshes the quest menu UI.
        /// </summary>
        /// <param name="quest">The quest to activate.</param>
        public void AddActiveQuest(QuestData quest)
        {
            ActivateQuestObjectives(quest);
            allActiveQuests.Add(quest);
            TrackQuest(quest);

            questMenuUI.RefreshQuestList();
        }


        /// <summary>
        /// Initializes quest objectives based on the quest info.
        /// Currently creates an <see cref="EnemyKillObjective"/> if enemies are required.
        /// </summary>
        /// <param name="quest">The quest for which to initialize objectives.</param>
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

        /// <summary>
        /// Adds the given quest to the tracking list and refreshes the tracking UI.
        /// </summary>
        /// <param name="quest">The quest to start tracking.</param>
        public void TrackQuest(QuestData quest)
        {
            allTrackingQuests.Add(quest);

            questMenuUI.RefreshTrackingList();
        }


        /// <summary>
        /// Removes the given quest from the tracking list and refreshes the tracking UI.
        /// </summary>
        /// <param name="quest">The quest to stop tracking.</param>
        public void UnTrackQuest(QuestData quest)
        {
            allTrackingQuests.Remove(quest);

            questMenuUI.RefreshTrackingList();
        }


        /// <summary>
        /// Updates objectives across all active quests based on the given ID,
        /// and logs when a quest is completed. Refreshes the tracking UI.
        /// </summary>
        /// <param name="id">
        /// Identifier of the updated entity: can be an enemyID (for kills)
        /// or an itemID (for item collection).
        /// </param>
        public void NotifyObjectiveProgress(string id)
        {
            foreach (var quest in allActiveQuests)
            {
                foreach (var objective in quest.questObjectives)
                {
                    objective.UpdateProgress(id);

                    if (objective.IsCompleted)
                    {
                        Debug.Log($"[Quest] '{quest.questInfoSO.questName}' completed");
                    }
                }
            }

            questMenuUI.RefreshTrackingList();
        }


        /// <summary>
        /// Event handler for enemy kill events.  
        /// Notifies objective progress with the killed enemy's ID.
        /// </summary>
        /// <param name="evt">The event data containing the enemy ID.</param>
        private void OnEnemyKilled(GameEvent.EnemyKilled evt)
        {
            NotifyObjectiveProgress(evt.enemyID);
        }


        /// <summary>
        /// Event handler for item collected events.  
        /// Notifies objective progress with the collected item's ID.
        /// </summary>
        /// <param name="evt">The event data containing the item ID.</param>
        private void OnItemCollected(GameEvent.ItemCollected evt)
        {
            NotifyObjectiveProgress(evt.itemID);
        }
        #endregion
    }
}
