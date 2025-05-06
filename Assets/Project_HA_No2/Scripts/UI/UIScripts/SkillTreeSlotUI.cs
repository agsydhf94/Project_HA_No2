using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HA
{
    public class SkillTreeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
    {
        public bool unlocked;

        private CanvasUI canvasUI;

        [SerializeField] private string skillName;
        [SerializeField] private int skillCost;
        [TextArea]
        [SerializeField] private string skillDescription;

        [SerializeField] private SkillTreeSlotUI[] shouldBeUnlocked;
        [SerializeField] private SkillTreeSlotUI[] shouldBeLocked;

        private Image skillImage;
        [SerializeField] private Color lockedSkillColor;

        private void OnValidate()
        {
            gameObject.name = "SkillTreeUI + " + skillName;
        }

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
        }

        private void Start()
        {
            canvasUI = GetComponentInParent<CanvasUI>();

            skillImage = GetComponent<Image>();
            skillImage.color = lockedSkillColor;    

            if(unlocked)
            {
                skillImage.color = Color.white;
            }
        }

        public void UnlockSkillSlot()
        {
            if (PlayerManager.Instance.CheckEnoughMoney(skillCost) == false)
                return;

            for(int i = 0; i < shouldBeUnlocked.Length; i++)
            {
                if (shouldBeUnlocked[i].unlocked == false)
                {
                    Debug.Log("스킬을 해금할 수 없습니다/");
                    return;
                }
            }

            for (int i = 0; i < shouldBeLocked.Length; i++)
            {
                if (shouldBeLocked[i].unlocked == false)
                {
                    Debug.Log("스킬을 해금할 수 없습니다/");
                    return;
                }
            }

            unlocked = true;
            skillImage.color = Color.white;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            canvasUI.skillToolTipUI.ShowToolTip(skillDescription, skillName, skillCost);          
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            canvasUI.skillToolTipUI.HideToolTip();
        }

        public void LoadData(GameData _data)
        {
            if (_data.skillTree.TryGetValue(skillName, out bool value))
            {
                unlocked = value;
            }
            
        }

        public void SaveData(ref GameData _data)
        {
            if(_data.skillTree.TryGetValue(skillName, out bool value))
            {
                _data.skillTree.Remove(skillName);
                _data.skillTree.Add(skillName, unlocked);
            }
            else
            {
                _data.skillTree.Add(skillName, unlocked);
            }
        }
    }
}

