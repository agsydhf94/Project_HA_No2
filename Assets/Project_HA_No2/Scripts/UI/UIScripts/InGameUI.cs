using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class InGameUI : MonoBehaviour
    {
        private PlayerManager playerManager;
        private SkillManager skillManager;
        private Inventory inventory;
        [SerializeField] private PlayerStat playerStat;
        [SerializeField] private Slider playerHPBar;

        [SerializeField] private Image dashSkillImage;
        [SerializeField] private Image counterAttackImage;
        [SerializeField] private Image elementSkillImage;
        [SerializeField] private Image ballSkillImage;
        [SerializeField] private Image blackHoleImage;
        [SerializeField] private Image potionImage;

        [SerializeField] private TMP_Text currentMoney;


        private void Awake()
        {
            playerManager = PlayerManager.Instance;
            skillManager = SkillManager.Instance;
            inventory = Inventory.Instance;
            playerStat.onHealthChanged += UpdateHealthUI;
        }

        private void Start()
        {            
            UpdateHealthUI();
        }

        private void Update()
        {
            currentMoney.text = playerManager.GetCurrentMoney().ToString("#,#");

            if(Input.GetKeyDown(KeyCode.C) && skillManager.dashSkill.dashUnlocked)
            {
                SetCooldownOf(dashSkillImage);
            }
            

            if(Input.GetKeyDown(KeyCode.Q) && skillManager.counterAttackSkill.counterAttackUnlocked)
            {
                SetCooldownOf(counterAttackImage);
            }
            

            if(Input.GetKeyDown(KeyCode.E) && skillManager.elementSkill.elementUnlocked)
            {
                SetCooldownOf(elementSkillImage);
            }

            
            if(Input.GetKeyDown(KeyCode.Mouse1) && skillManager.ballThrowSkill.ballThrowUnlocked)
            {
                SetCooldownOf(ballSkillImage);
            }
            

            if(Input.GetKeyDown(KeyCode.Z) && skillManager.blackHoleSkill.blackHoleUnlocked)
            {
                SetCooldownOf(blackHoleImage);
            }
            

            if(Input.GetKeyDown(KeyCode.Alpha8) && inventory.GetEquipment(EquipmentType.Potion) != null)
            {
                Debug.Log("포션 쿨다운");
                Debug.Log("포션 쿨다운 타임" + inventory.potionCooldown);
                SetCooldownOf(potionImage);
            }

            CheckCoolDownOf(dashSkillImage, skillManager.dashSkill.cooldown);
            CheckCoolDownOf(counterAttackImage, skillManager.counterAttackSkill.cooldown);
            CheckCoolDownOf(elementSkillImage, skillManager.elementSkill.cooldown);
            CheckCoolDownOf(ballSkillImage, skillManager.ballThrowSkill.cooldown);
            CheckCoolDownOf(blackHoleImage, skillManager.blackHoleSkill.cooldown);
            CheckCoolDownOf(potionImage, inventory.potionCooldown);
        }

        private void UpdateHealthUI()
        {
            playerHPBar.maxValue = playerStat.GetMaxHealthValue();
            playerHPBar.value = playerStat.currentHp;
        }

        private void SetCooldownOf(Image _image)
        {
            if(_image.fillAmount <= 0)
            {
                _image.fillAmount = 1;
            }
        }

        private void CheckCoolDownOf(Image _image, float _cooldown)
        {
            if(_image.fillAmount > 0)
            {
                _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
            }
        }
    }
}
