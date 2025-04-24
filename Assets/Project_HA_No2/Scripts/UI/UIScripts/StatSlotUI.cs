using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HA
{
    public class StatSlotUI : MonoBehaviour
    {
        [SerializeField] private string statName;
        private PlayerStat playerStat;

        [SerializeField] private StatType statType;
        [SerializeField] private TMP_Text statValueText;
        [SerializeField] private TMP_Text statNameText;

        private void OnValidate()
        {
            gameObject.name = "Stat - " + statName;

            if(statNameText != null)
            {
                statNameText.text = statName;
            }
        }

        private void Start()
        {
            playerStat = PlayerManager.Instance.playerCharacter.GetComponent<PlayerStat>();
            UpdateStatValueUI();
        }

        public void UpdateStatValueUI()
        {
            statValueText.text = playerStat.GetStat(statType).GetValue().ToString();
        }
    }
}
