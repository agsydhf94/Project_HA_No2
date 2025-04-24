using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HA
{
    public class StatToolTipUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text statDescription;

        public void ShowStatToolTip(string _text)
        {
            statDescription.text = _text;

            gameObject.SetActive(true);
        }

        public void HideStatToolTip()
        {
            statDescription.text = "";

            gameObject.SetActive(false);
        }
    }
}
