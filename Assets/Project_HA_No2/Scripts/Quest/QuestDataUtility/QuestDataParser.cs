using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HA
{
    public static class QuestDataParser
    {
        [System.Serializable]
        public class QuestRawData
        {
            public string questID;
            public string questName;
            public string questNPC;
            public string acceptOption;
            public string acceptAnswer;
            public string declineOption;
            public string declineAnswer;
            public string comebackAfterDecline;
            public string comebackInProgress;
            public string comebackFinished;
            public string lastDialog;
        }

        [System.Serializable]
        public class DialogueRawData
        {
            public string questID;
            public int lineIndex;
            public string dialogLine;
        }

        [System.Serializable]
        public class RequiredItemRawData
        {
            public string questID;
            public string itemID;
            public int amount;
        }

        [System.Serializable]
        public class RequiredEnemyRawData
        {
            public string questID;
            public string enemyID;
            public int amount;
        }

        [System.Serializable]
        public class RewardItemRawData
        {
            public string questID;
            public string itemID;
            public int amount;
        }

        public static QuestInfoSO ParseToSO(
            QuestRawData questRawData, 
            List<DialogueRawData> allDialoguesRawData, 
            List<RequiredItemRawData> allRequiredItems, 
            List<RequiredEnemyRawData> allRequiredEnemies,
            List<RewardItemRawData> allRewardItems, 
            Dictionary<string, ItemDataSO> itemDict,
            Dictionary<string, EnemyDataSO> enemyDict)
        {
            QuestInfoSO so = ScriptableObject.CreateInstance<QuestInfoSO>();

            // �⺻ ���� ����
            so.questID = questRawData.questID;
            so.questName = questRawData.questName;
            so.questNPC = questRawData.questNPC;

            so.acceptOption = questRawData.acceptOption;
            so.acceptAnswer = questRawData.acceptAnswer;
            so.declineOption = questRawData.declineOption;
            so.declineAnswer = questRawData.declineAnswer;
            so.comebackAfterDecline = questRawData.comebackAfterDecline;
            so.comebackInProgress = questRawData.comebackInProgress;
            so.comebackFinished = questRawData.comebackFinished;
            so.lastDialog = questRawData.lastDialog;

            // ��� ���͸� �� ����
            var filteredDialogues = allDialoguesRawData
                .Where(d => d.questID == questRawData.questID)
                .OrderBy(d => d.lineIndex)
                .ToList();

            // �ʱ� ��� ����Ʈ �Ҵ�
            so.initialDialogue = filteredDialogues.Select(d => d.dialogLine).ToList();

            // ����Ʈ �ϷḦ ���� ������ ���� ��� �Ҵ�
            so.requiredItems = new SerializableDictionary<ItemDataSO, int>();
            foreach (var entry in allRequiredItems.Where(i => i.questID == questRawData.questID))
            {
                if (string.IsNullOrEmpty(entry.itemID) || entry.itemID.ToLower() == "null" || entry.amount <= 0)
                {
                    Debug.Log($"[QuestParser] '{questRawData.questID}' ����Ʈ�� ���� ������ �䱸 ����");
                    continue;
                }

                if (itemDict.TryGetValue(entry.itemID, out var item))
                    so.requiredItems.Add(item, entry.amount);
                else
                    Debug.LogWarning($"[QuestParser] '{questRawData.questID}' ����Ʈ���� itemID '{entry.itemID}' �� ã�� ���߽��ϴ�.");
            }

            // ����Ʈ �ϷḦ ���� �� óġ ��� �Ҵ�
            so.requiredEnemies = new SerializableDictionary<EnemyDataSO, int>();
            foreach (var entry in allRequiredEnemies.Where(e => e.questID == questRawData.questID))
            {
                if (string.IsNullOrEmpty(entry.enemyID) || entry.enemyID.ToLower() == "null" || entry.amount <= 0)
                {
                    Debug.Log($"[QuestParser] '{questRawData.questID}' ����Ʈ�� �� óġ �䱸 ����");
                    continue;
                }

                if (enemyDict.TryGetValue(entry.enemyID, out var enemy))
                    so.requiredEnemies.Add(enemy, entry.amount);
                else
                    Debug.LogWarning($"[QuestParser] '{questRawData.questID}' ����Ʈ���� enemyID '{entry.enemyID}' �� ã�� ���߽��ϴ�.");
            }

            // ����Ʈ ���� ��� �Ҵ�
            so.rewardItems = new SerializableDictionary<ItemDataSO, int>();
            foreach (var reward in allRewardItems.Where(r => r.questID == questRawData.questID))
            {
                if (string.IsNullOrEmpty(reward.itemID) || reward.itemID.ToLower() == "null" || reward.amount <= 0)
                {
                    Debug.Log($"[QuestParser] '{questRawData.questID}' ����Ʈ�� ���� ������ �׸��� ���õ� (itemID: '{reward.itemID}', amount: {reward.amount})");
                    continue;
                }

                if (itemDict.TryGetValue(reward.itemID, out var rewardItem))
                    so.rewardItems.Add(rewardItem, reward.amount);
                else
                    Debug.LogWarning($"[QuestParser] reward itemID '{reward.itemID}' �� '{questRawData.questID}' ����Ʈ���� ã�� ���߽��ϴ�.");
            }

            return so;
        }
    }
}
