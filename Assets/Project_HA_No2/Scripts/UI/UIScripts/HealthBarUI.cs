using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class HealthBarUI : MonoBehaviour
    {
        private CharacterBase characterBase;
        private CharacterStats characterStats;
        private RectTransform rectTransform;
        private Slider slider;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            characterBase = GetComponentInParent<CharacterBase>();
            characterStats = GetComponentInParent<CharacterStats>();
            slider = GetComponentInChildren<Slider>();
        }

        private void Update()
        {
            UpdateHealthUI();
        }

        private void UpdateHealthUI()
        {
            slider.maxValue = characterStats.GetMaxHealthValue();
            slider.value = characterStats.currentHp;
        }
    }
}
