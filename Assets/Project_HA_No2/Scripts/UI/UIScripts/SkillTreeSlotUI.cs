using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HA
{
    /// <summary>
    /// UI component representing an individual skill slot in a skill tree.
    /// Handles unlocking logic, cost checking, tooltip display, and persistence.
    /// </summary>
    public class SkillTreeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
    {
        /// <summary>
        /// Indicates whether the skill has been unlocked.
        /// </summary>
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
        [SerializeField] private ImageRadialFiller imageRadialFiller;

        /// <summary>
        /// Auto-renames the GameObject in the editor for better organization.
        /// </summary>
        private void OnValidate()
        {
            gameObject.name = "SkillTreeUI + " + skillName;
        }


        /// <summary>
        /// Registers the button click listener for unlocking the skill.
        /// </summary>
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
        }


        /// <summary>
        /// Initializes references and sets initial skill image state.
        /// </summary>
        private void Start()
        {
            canvasUI = GetComponentInParent<CanvasUI>();

            skillImage = GetComponent<Image>();
            skillImage.color = lockedSkillColor;

            if (unlocked)
            {
                skillImage.color = Color.white;
                imageRadialFiller.Play();
            }
        }


        /// <summary>
        /// Attempts to unlock the skill slot after validating prerequisites and cost.
        /// </summary>
        public void UnlockSkillSlot()
        {
            if (PlayerManager.Instance.CheckEnoughMoney(skillCost) == false)
                return;

            if (unlocked)
                return;

            for (int i = 0; i < shouldBeUnlocked.Length; i++)
                {
                    if (shouldBeUnlocked[i].unlocked == false)
                    {
                        Debug.Log("This skill cannot be unlocked yet.");
                        return;
                    }
                }

            for (int i = 0; i < shouldBeLocked.Length; i++)
            {
                if (shouldBeLocked[i].unlocked == false)
                {
                    Debug.Log("This skill cannot be unlocked yet.");
                    return;
                }
            }

            unlocked = true;
            skillImage.color = Color.white;
            imageRadialFiller.Play();
        }


        /// <summary>
        /// Displays the tooltip when the pointer hovers over the skill.
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            canvasUI.skillToolTipUI.ShowToolTip(skillDescription, skillName, skillCost);
        }


        /// <summary>
        /// Hides the tooltip when the pointer exits the skill area.
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            canvasUI.skillToolTipUI.HideToolTip();
        }


        /// <summary>
        /// Loads saved data for this skill slot from the global game data.
        /// </summary>
        public void LoadData(GameData _data)
        {
            if (_data.skillTree.TryGetValue(skillName, out bool value))
            {
                unlocked = value;
            }

        }


        /// <summary>
        /// Saves the current state of this skill slot into the global game data.
        /// </summary>
        public void SaveData(ref GameData _data)
        {
            if (_data.skillTree.TryGetValue(skillName, out bool value))
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

