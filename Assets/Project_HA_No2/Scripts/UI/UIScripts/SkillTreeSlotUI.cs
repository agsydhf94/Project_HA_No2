using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HA
{
    public class SkillTreeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool unlocked;

        private CanvasUI canvasUI;

        [SerializeField] private string skillName;
        [SerializeField] private int skillPrice;
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
        }

        public void UnlockSkillSlot()
        {
            if (PlayerManager.Instance.CheckEnoughMoney(skillPrice) == false)
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
            canvasUI.skillToolTipUI.ShowToolTip(skillDescription, skillName);

            Vector2 mousePosition = Input.mousePosition;

            float xOffset = 0f;
            float yOffset = 0f;

            if(mousePosition.x > 600f)
                xOffset = -100f;
            else
                xOffset = 100f;

            if (mousePosition.y > 600f)
                yOffset = -60f;
            else
                yOffset = 60f;

            canvasUI.skillToolTipUI.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            canvasUI.skillToolTipUI.HideToolTip();
        }
    }
}

