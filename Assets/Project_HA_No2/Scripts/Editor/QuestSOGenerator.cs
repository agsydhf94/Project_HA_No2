#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.IO;
using System.Linq;

namespace HA
{
#if UNITY_EDITOR
    public class QuestSOGenerator : EditorWindow
    {
        private string excelFilePath = "Assets/Project_HA_No2/DataTables/QuestInfo.xlsx"; // 엑셀 경로
        private string savePath = "Assets/Project_HA_No2/ScriptableObjects/Quest";     // SO 저장 경로

        [MenuItem("Tools/Quest/Generate QuestInfoSO")]
        public static void ShowWindow()
        {
            GetWindow<QuestSOGenerator>("Quest SO Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Excel to QuestInfoSO Generator", EditorStyles.boldLabel);

            excelFilePath = EditorGUILayout.TextField("Excel File Path", excelFilePath);
            savePath = EditorGUILayout.TextField("Save Path", savePath);

            if (GUILayout.Button("Generate"))
            {
                GenerateQuestSOs();
            }
        }

        private async void GenerateQuestSOs()
        {
            if (!File.Exists(excelFilePath))
            {
                Debug.LogError($"Excel 파일이 존재하지 않습니다: {excelFilePath}");
                return;
            }

            // QuestData 역직렬화
            var questRawList = ExcelToJsonConverter.ConvertSheetToList<QuestDataParser.QuestRawData>(excelFilePath, "QuestData");
            var dialogs = ExcelToJsonConverter.ConvertSheetToList<QuestDataParser.DialogueRawData>(excelFilePath, "DialogueData");
            var requiredItems = ExcelToJsonConverter.ConvertSheetToList<QuestDataParser.RequiredItemRawData>(excelFilePath, "RequiredItems");
            var requiredEnemies = ExcelToJsonConverter.ConvertSheetToList<QuestDataParser.RequiredEnemyRawData>(excelFilePath, "RequiredEnemies");
            var rewardItems = ExcelToJsonConverter.ConvertSheetToList<QuestDataParser.RewardItemRawData>(excelFilePath, "RewardItems");

            var allItems = await AddressablesHelper.LoadAllSOByID<ItemDataSO>("ItemDataSO", "itemID");
            var allEnemies = await AddressablesHelper.LoadAllSOByID<EnemyDataSO>("EnemyDataSO", "enemyID");

            // 퀘스트별로 SO 생성
            foreach (var quest in questRawList)
            {
                var questSO = QuestDataParser.ParseToSO(quest, dialogs, requiredItems, requiredEnemies, rewardItems, allItems, allEnemies);
                Debug.Log($"[QuestSOGenerator] QuestID: {quest.questID}, rewardItems Count: {questSO.rewardItems?.Count ?? -1}");
                Debug.Log($"[QuestSOGenerator] QuestID: {quest.questID}, requiredEnemies Count: {questSO.requiredEnemies?.Count ?? -1}");
                string assetName = $"Quest_{quest.questID}.asset";
                string assetPath = $"{savePath}/{assetName}";


                // 디버그용 절대 경로 출력
                Debug.Log($"[QuestSOGenerator] Creating: {assetPath}");

                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                    Debug.Log($"[QuestSOGenerator] Created folder: {savePath}");
                }
                 
                

                AssetDatabase.CreateAsset(questSO, assetPath);
                Debug.Log($"[QuestSOGenerator] Created asset: {assetPath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("퀘스트 SO 생성 완료");
        }

    }
#endif
}