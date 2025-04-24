using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class CanvasUI : MonoBehaviour
    {
        public ItemToolTipUI itemToolTipUI;
        public StatToolTipUI statToolTipUI;

        public void SwitchUI(GameObject menu)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            if (menu != null)
            {
                menu.SetActive(true);
            }
        }
    }
}
