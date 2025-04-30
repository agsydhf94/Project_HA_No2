using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HA
{
    public class SkillToolTipUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text skillName;
        [SerializeField] private TMP_Text skillDescription;

        public void ShowToolTip(string _skillDescription, string _skillName)
        {
            skillName.text = _skillName;
            skillDescription.text = _skillDescription;
            gameObject.SetActive(true);
        }

        public void HideToolTip() => gameObject.SetActive(false);
    }
}
