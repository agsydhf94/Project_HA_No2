using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class QuestObjectivePrefab : MonoBehaviour
    {
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private Image checkBoxNo;
        [SerializeField] private Image checkBoxCompleted;

        public void Initialize(IQuestObjective questObjective)
        {
            progressText.text = questObjective.GetProgressDescription();
        }
    }
}
