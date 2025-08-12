using System.Collections.Generic;
using System.Linq;

namespace HA
{
    /// <summary>
    /// Tracks kill progress for a specific enemy type within a quest.
    /// Stores the enemy's unique ID, current kill count, target kill count,
    /// and provides a completion check.
    /// </summary>
    [System.Serializable]
    public class EnemyKillProgress
    {
        public string enemyID;
        public int currentKills;
        public int targetKills;

        /// <summary>
        /// True if the current kill count meets or exceeds the target.
        /// </summary>
        public bool isCompleted => currentKills >= targetKills;
    }

    /// <summary>
    /// Quest objective that requires the player to eliminate a set number of specific enemies.
    /// Initializes kill targets from <see cref="QuestInfoSO"/>, tracks kill progress,
    /// and updates a bound <see cref="QuestObjectiveViewModel"/> for UI display.
    /// </summary>
    public class EnemyKillObjective : IQuestObjective
    {

        private QuestObjectiveViewModel boundViewModel;
        public List<EnemyKillProgress> progressList = new();

        /// <summary>
        /// True if all tracked enemy kill requirements are completed.
        /// </summary>
        public bool IsCompleted => progressList.All(p => p.isCompleted);


        /// <summary>
        /// Initializes the kill progress list based on the quest's required enemies.
        /// Sets initial kill counts to zero.
        /// </summary>
        /// <param name="questInfo">Quest data containing enemy requirements.</param>
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


        /// <summary>
        /// Binds this objective to a <see cref="QuestObjectiveViewModel"/> so UI can be notified of updates.
        /// </summary>
        /// <param name="vm">The ViewModel to bind.</param>
        public void BindViewModel(QuestObjectiveViewModel vm)
        {
            boundViewModel = vm;
        }


        /// <summary>
        /// Increments the kill count for the specified enemy ID and notifies the bound ViewModel.
        /// </summary>
        /// <param name="enemyID">The ID of the enemy that was killed.</param>
        public void UpdateProgress(string enemyID)
        {
            foreach (var progress in progressList)
            {
                if (progress.enemyID == enemyID)
                {
                    progress.currentKills++;
                    boundViewModel?.Notify();
                    break;
                }
            }
        }


        /// <summary>
        /// Returns a formatted string describing the kill progress for each tracked enemy.
        /// Format: "enemyID: currentKills / targetKills" per line.
        /// </summary>
        public string GetProgressDescription()
        {
            var lines = progressList.Select(p =>
                $"{p.enemyID}: {p.currentKills} / {p.targetKills}");
            return string.Join("\n", lines);
        }
    }
}
