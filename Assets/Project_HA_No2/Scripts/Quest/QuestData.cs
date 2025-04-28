using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    [System.Serializable]
    public class QuestData
    {
        [Header("Check Bools")]
        public bool accepted;
        public bool declined;
        public bool initialDialogCompleted;
        public bool isCompleted;
        public bool hasRequirements;

        [Header("Quest Information")]
        public QuestInfoSO questInfoSO;
    }   
}
