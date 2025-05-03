using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HA
{
    public class SkillToolTipUI : ToolTipUI
    {
        [SerializeField] private TMP_Text skillName;
        [SerializeField] private TMP_Text skillDescription;
        [SerializeField] private TMP_Text skillCost;
        [SerializeField] private float defaultNameFontSize;

        public void ShowToolTip(string _skillDescription, string _skillName, int _skillCost)
        {
            skillName.text = _skillName;
            skillDescription.text = _skillDescription;
            skillCost.text = "Cost : " + _skillCost.ToString();

            AdjustPosition();
            AdjustFontSize(skillName);

            gameObject.SetActive(true);
        }

        public void HideToolTip()
        {
            skillName.fontSize = defaultNameFontSize;
            gameObject.SetActive(false);
        }
    }
}
