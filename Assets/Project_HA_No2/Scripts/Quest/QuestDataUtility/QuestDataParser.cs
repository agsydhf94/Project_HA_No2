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

            // 기본 정보 설정
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

            // 대사 필터링 및 정렬
            var filteredDialogues = allDialoguesRawData
                .Where(d => d.questID == questRawData.questID)
                .OrderBy(d => d.lineIndex)
                .ToList();

            // 초기 대사 리스트 할당
            so.initialDialogue = filteredDialogues.Select(d => d.dialogLine).ToList();

            // 퀘스트 완료를 위한 아이템 수집 목록 할당
            so.requiredItems = new SerializableDictionary<ItemDataSO, int>();
            foreach (var entry in allRequiredItems.Where(i => i.questID == questRawData.questID))
            {
                if (string.IsNullOrEmpty(entry.itemID) || entry.itemID.ToLower() == "null" || entry.amount <= 0)
                {
                    Debug.Log($"[QuestParser] '{questRawData.questID}' 퀘스트는 수집 아이템 요구 없음");
                    continue;
                }

                if (itemDict.TryGetValue(entry.itemID, out var item))
                    so.requiredItems.Add(item, entry.amount);
                else
                    Debug.LogWarning($"[QuestParser] '{questRawData.questID}' 퀘스트에서 itemID '{entry.itemID}' 를 찾지 못했습니다.");
            }

            // 퀘스트 완료를 위한 적 처치 목록 할당
            so.requiredEnemies = new SerializableDictionary<EnemyDataSO, int>();
            foreach (var entry in allRequiredEnemies.Where(e => e.questID == questRawData.questID))
            {
                if (string.IsNullOrEmpty(entry.enemyID) || entry.enemyID.ToLower() == "null" || entry.amount <= 0)
                {
                    Debug.Log($"[QuestParser] '{questRawData.questID}' 퀘스트는 적 처치 요구 없음");
                    continue;
                }

                if (enemyDict.TryGetValue(entry.enemyID, out var enemy))
                    so.requiredEnemies.Add(enemy, entry.amount);
                else
                    Debug.LogWarning($"[QuestParser] '{questRawData.questID}' 퀘스트에서 enemyID '{entry.enemyID}' 를 찾지 못했습니다.");
            }

            // 퀘스트 보상 목록 할당
            so.rewardItems = new SerializableDictionary<ItemDataSO, int>();
            foreach (var reward in allRewardItems.Where(r => r.questID == questRawData.questID))
            {
                if (string.IsNullOrEmpty(reward.itemID) || reward.itemID.ToLower() == "null" || reward.amount <= 0)
                {
                    Debug.Log($"[QuestParser] '{questRawData.questID}' 퀘스트의 보상 아이템 항목이 무시됨 (itemID: '{reward.itemID}', amount: {reward.amount})");
                    continue;
                }

                if (itemDict.TryGetValue(reward.itemID, out var rewardItem))
                    so.rewardItems.Add(rewardItem, reward.amount);
                else
                    Debug.LogWarning($"[QuestParser] reward itemID '{reward.itemID}' 를 '{questRawData.questID}' 퀘스트에서 찾지 못했습니다.");
            }

            return so;
        }
    }
}
