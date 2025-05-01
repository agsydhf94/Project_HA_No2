using UnityEngine;

namespace HA
{
    public class CanvasUI : MonoBehaviour
    {
        [SerializeField] private GameObject characterUI;
        [SerializeField] private GameObject skillTreeUI;
        [SerializeField] private GameObject craftUI;
        [SerializeField] private GameObject optionsUI;
        [SerializeField] private GameObject inGameUI;

        public ItemToolTipUI itemToolTipUI;
        public StatToolTipUI statToolTipUI;
        public CraftWindowUI craftWindowUI;
        public SkillToolTipUI skillToolTipUI;

        private void Awake()
        {
            // ��ųƮ�� ���Կ� �̺�Ʈ�� ����ϱ� ���� ����
            SwitchUITo(skillTreeUI);
        }

        private void Start()
        {
            SwitchUI(null);
            SwitchUITo(inGameUI);

            itemToolTipUI.gameObject.SetActive(false);
            statToolTipUI.gameObject.SetActive(false);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchUITo(characterUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchUITo(skillTreeUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchUITo(craftUI);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SwitchUITo(optionsUI);
            }
        }

        public void SwitchUI(GameObject menu)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            if (menu != null)
            {
                menu.SetActive(true);
            }
        }

        public void SwitchUITo(GameObject menuUI)
        {
            if(menuUI != null && menuUI.activeSelf)
            {
                menuUI.SetActive(false);
                CheckForInGameUI();

                return;
            }

            SwitchUI(menuUI);
        }

        private void CheckForInGameUI()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                    return;
            }

            SwitchUITo(inGameUI);
        }
    }
}
