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

        private void Start()
        {
            canvasUI = GetComponentInParent<CanvasUI>();

            skillImage = GetComponent<Image>();

            skillImage.color = lockedSkillColor;

            GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
        }

        public void UnlockSkillSlot()
        {
            for(int i = 0; i < shouldBeUnlocked.Length; i++)
            {
                if (shouldBeUnlocked[i].unlocked == false)
                {
                    Debug.Log("��ų�� �ر��� �� �����ϴ�/");
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

