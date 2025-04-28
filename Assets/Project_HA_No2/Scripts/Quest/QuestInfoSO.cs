using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    [CreateAssetMenu(fileName = "DataSO", menuName = "Quest/QuestInformation")]
    public class QuestInfoSO : ScriptableObject
    {
        public List<string> initialDialogue;

        [Header("Check Options")]
        public string acceptOption;
        public string acceptAnswer;
        public string declineOption;
        public string declineAnswer;
        public string comebackAfterDecline;
        public string comebackInProgress;
        public string comebackFinished;
        public string lastDialouge;

        [Header("Quest Requirements for finish")]
        public Dictionary<ItemDataSO, int> requiredItem_and_Amount;

        [Header("Quest Rewards")]
        public int rewardMoney;
        public List<ItemDataSO> rewardItems;
    }
}
