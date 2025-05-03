using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HA
{
    public class StatToolTipUI : ToolTipUI
    {
        [SerializeField] private TMP_Text statDescription;

        public void ShowStatToolTip(string _text)
        {
            statDescription.text = _text;
            AdjustPosition();

            gameObject.SetActive(true);
        }

        public void HideStatToolTip()
        {
            statDescription.text = "";

            gameObject.SetActive(false);
        }
    }
}
