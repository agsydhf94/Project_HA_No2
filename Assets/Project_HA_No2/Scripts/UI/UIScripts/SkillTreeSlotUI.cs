using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class SkillTreeSlotUI : MonoBehaviour
    {
        public bool unlocked;

        [SerializeField] private SkillTreeSlotUI[] shouldBeUnlocked;
        [SerializeField] private SkillTreeSlotUI[] shouldBeLocked;

        [SerializeField] private Image skillImage;

        private void Start()
        {
            skillImage = GetComponent<Image>();

            skillImage.color = Color.red;

            GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
        }

        public void UnlockSkillSlot()
        {
            for(int i = 0; i < shouldBeUnlocked.Length; i++)
            {
                if (shouldBeUnlocked[i].unlocked == false)
                {
                    Debug.Log("스킬을 해금할 수 없습니다/");
                    return;
                }
            }

            unlocked = true;
            skillImage.color = Color.green;
        }
    }
}

