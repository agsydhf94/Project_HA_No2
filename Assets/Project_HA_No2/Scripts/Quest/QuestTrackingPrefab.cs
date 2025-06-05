using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HA
{
    public class QuestTrackingPrefab : MonoBehaviour
    {
        [SerializeField] private TMP_Text questNameText;
        [SerializeField] private GameObject objectiveRoot;
        [SerializeField] private QuestObjectivePrefab questTrackingObjectivesPrefab;

        public void Initialize(QuestData quest)
        {
            questNameText.text = quest.questInfoSO.questName;

            foreach(IQuestObjective questObjective in quest.questObjectives)
            {
                var prefabInstance = Instantiate(questTrackingObjectivesPrefab, objectiveRoot.transform);
                var objectivePrefabComponent = prefabInstance.GetComponent<QuestObjectivePrefab>();

                objectivePrefabComponent.Initialize(questObjective);
            }
        }
    }
}
