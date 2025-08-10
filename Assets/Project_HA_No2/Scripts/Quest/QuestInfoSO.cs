using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// ScriptableObject that stores all quest-related information, including:
    /// - Quest title and NPC information
    /// - Initial dialogue and various dialogue options
    /// - Required items and enemies to complete the quest
    /// - Reward items upon completion
    /// </summary>
    [CreateAssetMenu(fileName = "DataSO", menuName = "Quest/QuestInformation")]
    public class QuestInfoSO : ScriptableObject
    {
        public string questID;

        [Header("Quest Title")]
        public string questName;
        public string questNPC;

        [Header("Initial Dialog")]
        [TextArea(5, 10)]
        public List<string> initialDialogue;
    
        [Header("Check Options")]
        [TextArea(5, 10)] public string acceptOption;
        [TextArea(5, 10)] public string acceptAnswer;
        [TextArea(5, 10)] public string declineOption;
        [TextArea(5, 10)] public string declineAnswer;
        [TextArea(5, 10)] public string comebackAfterDecline;
        [TextArea(5, 10)] public string comebackInProgress;
        [TextArea(5, 10)] public string comebackFinished;
        [TextArea(5, 10)] public string lastDialog;

        [Header("Quest Requirements Item")]
        public SerializableDictionary<ItemDataSO, int> requiredItems;

        [Header("Quest Requirememts Eliminating Enemy")]
        public SerializableDictionary<EnemyDataSO, int> requiredEnemies;

        [Header("Quest Rewards")]
        public SerializableDictionary<ItemDataSO, int> rewardItems;
    }
}
