using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace HA
{
    public class StatSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private CanvasUI canvasUI;

        [SerializeField] private string statName;
        private PlayerStat playerStat;

        [SerializeField] private StatType statType;
        [SerializeField] private TMP_Text statValueText;
        [SerializeField] private TMP_Text statNameText;

        [TextArea]
        [SerializeField] private string statDescription;

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
            canvasUI = GetComponentInParent<CanvasUI>();
            UpdateStatValueUI();
        }

        public void UpdateStatValueUI()
        {
            statValueText.text = playerStat.GetStat(statType).GetValue().ToString();

            if(statType == StatType.Health)
                statValueText.text = playerStat.GetMaxHealthValue().ToString();

            if (statType == StatType.Damage)
                statValueText.text = (playerStat.damage.GetValue() + playerStat.strength.GetValue()).ToString();

            if (statType == StatType.CriticalPower)
                statValueText.text = (playerStat.criticalPower.GetValue() + playerStat.strength.GetValue()).ToString();

            if(statType == StatType.CriticalChance)
                statValueText.text = (playerStat.criticalChance.GetValue() + playerStat.agility.GetValue()).ToString();

            if (statType == StatType.Evasion)
                statValueText.text = (playerStat.evasion.GetValue() + playerStat.agility.GetValue()).ToString();

            if(statType == StatType.MagicResistance)
                statValueText.text = (playerStat.magicResistance.GetValue() + playerStat.inteligence.GetValue()).ToString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            canvasUI.statToolTipUI.ShowStatToolTip(statDescription);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            canvasUI.statToolTipUI.HideStatToolTip();
        }
    }
}
