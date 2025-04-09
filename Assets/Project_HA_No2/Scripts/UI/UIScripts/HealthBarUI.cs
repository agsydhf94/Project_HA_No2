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
            slider = GetComponentInChildren<Slider>();
            
            characterStats = GetComponentInParent<CharacterStats>();
            characterStats.onHealthChanged += UpdateHealthUI;
        }

        private void UpdateHealthUI()
        {
            slider.maxValue = characterStats.GetMaxHealthValue();
            slider.value = characterStats.currentHp;
        }

        private void OnDisable()
        {
            characterStats.onHealthChanged -= UpdateHealthUI;
        }
    }
}
