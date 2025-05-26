using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    [CreateAssetMenu(fileName = "DataSO", menuName = "Quest/QuestInformation")]
    public class QuestInfoSO : ScriptableObject
    {
        [TextArea(5, 10)]
        public List<string> initialDialogue;

        [Header("Check Options")]
        [TextArea(5, 10)]
        public string acceptOption;
        [TextArea(5, 10)]
        public string acceptAnswer;
        [TextArea(5, 10)]
        public string declineOption;
        [TextArea(5, 10)]
        public string declineAnswer;
        [TextArea(5, 10)]
        public string comebackAfterDecline;
        [TextArea(5, 10)]
        public string comebackInProgress;
        [TextArea(5, 10)]
        public string comebackFinished;
        [TextArea(5, 10)]
        public string lastDialouge;

        [Header("Quest Requirements Item")]
        public SerializableDictionary<ItemDataSO, int> requiredItem_and_Amount;

        [Header("Quest Requirememts Eliminating Enemy")]
        public SerializableDictionary<Enemy, int> requiredEnemy_and_Amount;

        [Header("Quest Rewards")]
        public int rewardMoney;
        public List<ItemDataSO> rewardItems;
    }
}
