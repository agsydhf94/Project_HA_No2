using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    /// <summary>
    /// Represents an NPC that can interact with the player,
    /// handle quests, and manage dialog and rewards.
    /// </summary>
    public class NPC : MonoBehaviour, IInteractable
    {
        [Header("Compoments")]
        /// <summary>
        /// Reference to the PlayerManager for accessing player state.
        /// </summary>
        private PlayerManager playerManager;

        /// <summary>
        /// Reference to the CanvasUI for dialog and quest UI.
        /// </summary>
        private CanvasUI canvasUI;

        [Header("Dialog")]

        /// <summary>
        /// Indicates whether the player is within interaction range.
        /// </summary>
        public bool isPlayerDetected;

        /// <summary>
        /// Indicates whether this NPC is currently in conversation with the player.
        /// </summary>
        public bool isTalking;

        /// <summary>
        /// The name of this NPC.
        /// </summary>
        public string npcName;


        private TMP_Text npcDialogText;
        private Button nextPageButton;
        private TMP_Text nextPageButtonText;
        private Button option1Button;
        private TMP_Text option1ButtonText;
        private Button option2Button;
        private TMP_Text option2ButtonText;

        [Header("Quest")]

        /// <summary>
        /// List of quests available from this NPC.
        /// </summary>
        public List<QuestData> quests;

        /// <summary>
        /// The quest currently being offered or tracked by this NPC.
        /// </summary>
        public QuestData currentActiveQuest = null;

        /// <summary>
        /// Index of the currently active quest in the list.
        /// </summary>
        public int activeQuestIndex = 0;

        /// <summary>
        /// Whether the player is interacting with this NPC for the first time.
        /// </summary>
        public bool firstTimeInteraction = true;

        /// <summary>
        /// The index of the current dialog line being displayed.
        /// </summary>
        public int currentDialogIndex;

        private void Awake()
        {
            playerManager = PlayerManager.Instance;
            canvasUI = CanvasUI.Instance;
        }


        /// <summary>
        /// Initializes references to dialog UI elements.
        /// </summary>
        private void Start()
        {
            npcDialogText = canvasUI.dialogUI.diaogText;

            nextPageButton = canvasUI.dialogUI.nextPageBtn;
            nextPageButtonText = canvasUI.dialogUI.nextPageBtn.GetComponentInChildren<TMP_Text>();

            option1Button = canvasUI.dialogUI.option1Btn;
            option1ButtonText = canvasUI.dialogUI.option1Btn.GetComponentInChildren<TMP_Text>();

            option2Button = canvasUI.dialogUI.option2Btn;
            option2ButtonText = canvasUI.dialogUI.option2Btn.GetComponentInChildren<TMP_Text>();
        }


        /// <summary>
        /// Returns the display name of this interactable NPC.
        /// </summary>
        /// <returns>The NPC's name.</returns>
        public string GetInteractableName()
        {
            return npcName;
        }


        /// <summary>
        /// Starts a conversation with the player, handling quest dialogs
        /// depending on the current quest state and interaction history.
        /// </summary>
        public void StartConversation()
        {
            isTalking = true;
            canvasUI.dialogUI.npcNameText.text = npcName;
            LookAtPlayer();

            // Interact with this NPC for the first time
            if (firstTimeInteraction == true)
            {
                firstTimeInteraction = false;
                currentActiveQuest = quests[activeQuestIndex];
                StartQuestInitialDialog();
                currentDialogIndex = 0;
            }
            else // Interact with this NPC who have met before
            {
                if (currentActiveQuest.declined == true)
                {
                    canvasUI.OpenDialogUI();

                    //npcDialogText.text = currentActiveQuest.questInfoSO.comebackAfterDecline;
                    canvasUI.dialogUI.typer.TypeText(currentActiveQuest.questInfoSO.comebackAfterDecline).Forget();

                    SetAcceptAndDeclineOptions();
                }

                if (currentActiveQuest.accepted && currentActiveQuest.isCompleted == false)
                {
                    if (CheckQuestCompleted())
                    {
                        HandoverRequireItems();

                        canvasUI.OpenDialogUI();

                        //npcDialogText.text = currentActiveQuest.questInfoSO.comebackFinished;
                        canvasUI.dialogUI.typer.TypeText(currentActiveQuest.questInfoSO.comebackFinished).Forget();
                        nextPageButton.onClick.RemoveAllListeners();
                        nextPageButton.onClick.AddListener(() =>
                        {
                            ReceiveRewardAndCompleteQuest();
                        });
                    }
                    else
                    {
                        canvasUI.OpenDialogUI();

                        //npcDialogText.text = currentActiveQuest.questInfoSO.comebackInProgress;
                        canvasUI.dialogUI.typer.TypeText(currentActiveQuest.questInfoSO.comebackInProgress).Forget();
                        option1ButtonText.text = "Close";
                        nextPageButton.onClick.RemoveAllListeners();
                        nextPageButton.onClick.AddListener(() =>
                        {
                            canvasUI.CloseDialogUI();
                            isTalking = false;
                        });
                    }
                }

                if (currentActiveQuest.isCompleted == true)
                {
                    canvasUI.OpenDialogUI();

                    //npcDialogText.text = currentActiveQuest.questInfoSO.lastDialog;
                    canvasUI.dialogUI.typer.TypeText(currentActiveQuest.questInfoSO.lastDialog).Forget();
                    option1ButtonText.text = "Close";
                    nextPageButton.onClick.RemoveAllListeners();
                    nextPageButton.onClick.AddListener(() =>
                    {
                        canvasUI.CloseDialogUI();
                        isTalking = false;
                    });
                }

                if (currentActiveQuest.initialDialogCompleted == false)
                {
                    StartQuestInitialDialog();
                }
            }
        }


        /// <summary>
        /// Checks whether all objectives of the current quest are completed.
        /// </summary>
        /// <returns>True if completed, false otherwise.</returns>
        private bool CheckQuestCompleted()
        {
            foreach (var objectives in currentActiveQuest.questObjectives)
            {
                if (!objectives.IsCompleted)
                    return false;

            }

            return true;
        }


        /// <summary>
        /// Removes the required items from the player's inventory for the quest handover.
        /// </summary>
        private void HandoverRequireItems()
        {
            var requiredItems = currentActiveQuest.questInfoSO.requiredItems;

            foreach (var pair in requiredItems)
            {
                ItemDataSO item = pair.Key;
                int requiredAmount = pair.Value;

                int removedCount = 0;

                while (removedCount < requiredAmount)
                {
                    Inventory.Instance.RemoveItem(item);
                    removedCount++;
                }
            }

        }


        /// <summary>
        /// Grants quest rewards to the player and marks the quest as completed.
        /// Handles transitioning to the next quest if available.
        /// </summary>
        private void ReceiveRewardAndCompleteQuest()
        {
            QuestManager.Instance.HandoverQuestCompleted(currentActiveQuest);

            currentActiveQuest.isCompleted = true;

            var rewardItems = currentActiveQuest.questInfoSO.rewardItems;

            foreach (var item in rewardItems)
            {
                if (item.Key == null)
                    continue;

                Inventory.Instance.AddItem(item.Key);
                activeQuestIndex++;
            }

            // Ready Next Quset
            if (activeQuestIndex < quests.Count)
            {
                currentActiveQuest = quests[activeQuestIndex];
                currentDialogIndex = 0;
                canvasUI.CloseDialogUI();
                isTalking = false;
            }
            else
            {
                canvasUI.CloseDialogUI();
                isTalking = false;
                Debug.Log("No More Quests for this NPC");
            }

            string titleText = "Quest Rewards";
            canvasUI.OpenRewardUI(titleText);
            canvasUI.rewardUI.SetRewardsAsync(rewardItems).Forget();

        }


        /// <summary>
        /// Begins the initial dialog sequence for the current quest.
        /// </summary>
        private void StartQuestInitialDialog()
        {
            canvasUI.OpenDialogUI();

            //npcDialogText.text = currentActiveQuest.questInfoSO.initialDialogue[currentDialog];
            canvasUI.dialogUI.typer.TypeText(currentActiveQuest.questInfoSO.initialDialogue[currentDialogIndex]).Forget();
            nextPageButtonText.text = "Next";
            nextPageButton.onClick.RemoveAllListeners();
            nextPageButton.onClick.AddListener(() =>
            {
                currentDialogIndex++;
                CheckIfDialogFinished();
            });

            option1Button.gameObject.SetActive(false);
            option2Button.gameObject.SetActive(false);
        }


        /// <summary>
        /// Checks if the initial dialog has finished and sets up options accordingly.
        /// </summary>
        private void CheckIfDialogFinished()
        {
            if (currentDialogIndex == currentActiveQuest.questInfoSO.initialDialogue.Count - 1)
            {
                //npcDialogText.text = currentActiveQuest.questInfoSO.initialDialogue[currentDialog];
                canvasUI.dialogUI.typer.TypeText(currentActiveQuest.questInfoSO.initialDialogue[currentDialogIndex]).Forget();

                currentActiveQuest.initialDialogCompleted = true;

                SetAcceptAndDeclineOptions();
            }
            else // The case if there are more dialogss
            {
                //npcDialogText.text = currentActiveQuest.questInfoSO.initialDialogue[currentDialog];
                canvasUI.dialogUI.typer.TypeText(currentActiveQuest.questInfoSO.initialDialogue[currentDialogIndex]).Forget();

                //option1ButtonText.text = "Next";
                nextPageButton.onClick.RemoveAllListeners();
                nextPageButton.onClick.AddListener(() =>
                {
                    currentDialogIndex++;
                    CheckIfDialogFinished();
                });
            }
        }


        /// <summary>
        /// Marks the current quest as accepted and updates dialog/UI accordingly.
        /// </summary>
        private void AcceptedQuest()
        {
            QuestManager.Instance.AddActiveQuest(currentActiveQuest);

            currentActiveQuest.accepted = true;
            currentActiveQuest.declined = false;

            if (currentActiveQuest.hasRequirements == true)
            {
                //npcDialogText.text = currentActiveQuest.questInfoSO.comebackFinished;
                canvasUI.dialogUI.typer.TypeText(currentActiveQuest.questInfoSO.comebackFinished).Forget();
                nextPageButtonText.text = "< Take Award >";
                nextPageButton.onClick.RemoveAllListeners();
                nextPageButton.onClick.AddListener(() =>
                {
                    ReceiveRewardAndCompleteQuest();
                });

                option1Button.gameObject.SetActive(false);
                option2Button.gameObject.SetActive(false);
            }
            else
            {
                //npcDialogText.text = currentActiveQuest.questInfoSO.acceptAnswer;
                canvasUI.dialogUI.typer.TypeText(currentActiveQuest.questInfoSO.acceptAnswer).Forget();
                CloseDialogUI_ForNPC();
            }
        }


        /// <summary>
        /// Marks the current quest as declined and updates dialog/UI accordingly.
        /// </summary>
        private void DeclinedQuest()
        {
            currentActiveQuest.declined = true;

            //npcDialogText.text = currentActiveQuest.questInfoSO.declineAnswer;
            canvasUI.dialogUI.typer.TypeText(currentActiveQuest.questInfoSO.declineAnswer).Forget();
            nextPageButtonText.text = "Close";
            CloseDialogUI_ForNPC();
        }


        /// <summary>
        /// Configures accept and decline options in the dialog UI for the current quest.
        /// </summary>
        private void SetAcceptAndDeclineOptions()
        {
            option1Button.gameObject.SetActive(true);
            option1ButtonText.text = currentActiveQuest.questInfoSO.acceptOption;
            option1Button.onClick.RemoveAllListeners();
            option1Button.onClick.AddListener(() =>
            {
                AcceptedQuest();
            });

            option2Button.gameObject.SetActive(true);
            option2ButtonText.text = currentActiveQuest.questInfoSO.declineOption;
            option2Button.onClick.RemoveAllListeners();
            option2Button.onClick.AddListener(() =>
            {
                DeclinedQuest();
            });
        }


        /// <summary>
        /// Closes the dialog UI from the NPC side and resets conversation state.
        /// </summary>
        private void CloseDialogUI_ForNPC()
        {
            nextPageButtonText.text = "Close";
            nextPageButton.onClick.RemoveAllListeners();
            nextPageButton.onClick.AddListener(() =>
            {
                canvasUI.CloseDialogUI();
                isTalking = false;
            });
            option1Button.gameObject.SetActive(false);
            option2Button.gameObject.SetActive(false);
        }


        /// <summary>
        /// Rotates the NPC to face the player character.
        /// </summary>
        private void LookAtPlayer()
        {
            PlayerCharacter player = playerManager.playerCharacter;
            Vector3 direction = player.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);

            var yRotation = transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }


        /// <summary>
        /// Called when the player enters this NPC's trigger zone.
        /// Sets the detection flag to true.
        /// </summary>
        /// <param name="other">The collider entering the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerDetected = true;
            }
        }


        /// <summary>
        /// Called when the player exits this NPC's trigger zone.
        /// Sets the detection flag to false.
        /// </summary>
        /// <param name="other">The collider exiting the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerDetected = false;
            }
        }
    }
}
