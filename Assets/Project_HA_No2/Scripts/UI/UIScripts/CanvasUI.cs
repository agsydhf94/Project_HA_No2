using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

namespace HA
{
    public class CanvasUI : SingletonBase<CanvasUI>
    {
        /// <summary>
        /// Callback invoked when the confirm UI's "Yes" button is pressed.
        /// Designed as a generic confirm callback.
        private Action OnYes;

        /// <summary>
        /// Callback invoked when the confirm UI's "No" button is pressed.
        /// Designed as a generic confirm callback.
        /// </summary>
        private Action OnNo;
        [Header("End Screen")]
        [SerializeField] private FadeScreenUI fadeScreenUI;
        [SerializeField] private GameObject endText;
        [SerializeField] private GameObject restartButton;

        [Header("Scene Change Screen")]
        [SerializeField] public WarpConfirmUI warpConfirmUI;
        [Header("Dialog Screen")]
        [SerializeField] public DialogUI dialogUI;

        [SerializeField] public CrosshairUI crosshairUI;
        [SerializeField] private GameObject characterUI;
        [SerializeField] private GameObject skillTreeUI;
        [SerializeField] private GameObject craftUI;
        [SerializeField] private GameObject optionsUI;
        [SerializeField] private GameObject inGameUI;

        public ItemToolTipUI itemToolTipUI;
        public StatToolTipUI statToolTipUI;
        public CraftWindowUI craftWindowUI;
        public SkillToolTipUI skillToolTipUI;

        private InputSystem inputSystem;

        public override void Awake()
        {
            // 스킬트리 슬롯에 이벤트를 등록하기 위해 존재
            SwitchUITo(skillTreeUI);

            inputSystem = InputSystem.Instance;
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
            if (dialogUI.dialogActive)
                return;

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

        public void OpenDialogUI()
        {
            dialogUI.gameObject.SetActive(true);
            dialogUI.dialogActive = true;

            inputSystem.isShowCursor = true;
            inputSystem.isInputBlocked = true;
        }

        public void CloseDialogUI()
        {
            dialogUI.gameObject.SetActive(false);
            dialogUI.dialogActive = false;

            inputSystem.isShowCursor = false;
            inputSystem.isInputBlocked = false;
        }

        /// <summary>
        /// Opens the warp confirmation UI and assigns callbacks for Yes/No responses.
        /// </summary>
        /// <param name="confirmYesCallback">
        /// Callback invoked when the user confirms (presses "Yes").
        /// </param>
        /// <param name="confirmNoCallback">
        /// Optional callback invoked when the user cancels (presses "No").
        /// </param>
        public void OpenWarpConfirmUI(Action confirmYesCallback, Action confirmNoCallback = null)
        {
            warpConfirmUI.gameObject.SetActive(true);
            warpConfirmUI.uIFadeScaler.PlayShow();

            OnYes = confirmYesCallback;
            OnNo = confirmNoCallback;
        }


        /// <summary>
        /// Closes the warp confirmation UI and clears assigned callbacks.
        /// </summary>
        public void CloseWarpConfirmUI()
        {
            warpConfirmUI.uIFadeScaler.PlayHide();
            OnYes = null;
            OnNo = null;
        }


        /// <summary>
        /// Handles Yes button click: closes the UI and invokes the assigned callback.
        /// </summary>
        public void WarpUI_OnClickYes()
        {
            CloseWarpConfirmUI();
            OnYes?.Invoke();
        }


        /// <summary>
        /// Handles No button click: closes the UI and invokes the assigned callback.
        /// </summary>
        public void WarpUI_OnClickNo()
        {
            CloseWarpConfirmUI();
            OnNo?.Invoke();
        }
        public void SwitchOnEndScreen()
        {
            fadeScreenUI.FadeOut();
            StartCoroutine(EndScreenCorutine());
        }

        IEnumerator EndScreenCorutine()
        {
            yield return new WaitForSeconds(1);
            endText.SetActive(true);

            yield return new WaitForSeconds(1);
            restartButton.SetActive(true);
        }

        public void RestartGameButton() => GameManager.Instance.RestartScene();

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
