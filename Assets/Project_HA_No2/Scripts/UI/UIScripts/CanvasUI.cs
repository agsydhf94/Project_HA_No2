using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

namespace HA
{
    public class CanvasUI : MonoBehaviour
    {
        [Header("End Screen")]
        [SerializeField] private FadeScreenUI fadeScreenUI;
        [SerializeField] private GameObject endText;


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
            // 스킬트리 슬롯에 이벤트를 등록하기 위해 존재
            SwitchUITo(skillTreeUI);
        }

        private void Start()
        {
            SwitchUI(null);
            //SwitchUITo(inGameUI);

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
                bool isFadeScreen = transform.GetChild(i).GetComponent<FadeScreenUI>() != null;

                if (isFadeScreen == false)
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
                //CheckForInGameUI();

                return;
            }

            SwitchUI(menuUI);
        }

        public void SwitchOnEndScreen()
        {
            SwitchUITo(null);
            fadeScreenUI.FadeOut();
        }

        IEnumerator EndScreenCorutine()
        {
            yield return new WaitForSeconds(1);

            endText.SetActive(true);
        }

        /*
        private void CheckForInGameUI()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                    return;
            }

            SwitchUITo(inGameUI);
        }
        */
    }
}
