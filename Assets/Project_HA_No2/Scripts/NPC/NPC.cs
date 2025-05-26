using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class NPC : MonoBehaviour, IInteractable
    {
        [Header("Compoments")]
        private PlayerManager playerManager;
        private CanvasUI canvasUI;

        [Header("Dialog")]
        public bool isPlayerDetected;
        public bool isTalking;
        public string npcName;
        private TMP_Text npcDialogText;
        private Button option1Button;
        private TMP_Text option1ButtonText;
        private Button option2Button;
        private TMP_Text option2ButtonText;

        [Header("Quest")]
        public List<QuestData> quests;
        public QuestData currentActiveQuest = null;
        public int activeQuestIndex = 0;
        public bool firstTimeInteraction = true;
        public int currentDialog;

        private void Awake()
        {
            playerManager = PlayerManager.Instance;
            canvasUI = CanvasUI.Instance;
        }

        private void Start()
        {
            npcDialogText = canvasUI.dialogUI.diaogText;

            option1Button = canvasUI.dialogUI.option1Btn;
            option1ButtonText = canvasUI.dialogUI.option1Btn.GetComponentInChildren<TMP_Text>();

            option2Button = canvasUI.dialogUI.option2Btn;
            option2ButtonText = canvasUI.dialogUI.option2Btn.GetComponentInChildren<TMP_Text>();
        }

        public string GetInteractableName()
        {
            return npcName;
        }

        public void StartConversation()
        {
            isTalking = true;
            LookAtPlayer();

            // Interact with this NPC for the first time
            if(firstTimeInteraction == true)
            {
                firstTimeInteraction = false;
                currentActiveQuest = quests[activeQuestIndex];
                StartQuestInitialDialog();
                currentDialog = 0;
            }
            else // Interact with this NPC who have met before
            {
                if(currentActiveQuest.declined == true)
                {
                    canvasUI.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.questInfoSO.comebackAfterDecline;
                    
                    SetAcceptAndDeclineOptions();
                }

                if (currentActiveQuest.accepted && currentActiveQuest.isCompleted == false)
                {
                    if(CheckQuestCompleted())
                    {
                        HandoverRequireItems();

                        canvasUI.OpenDialogUI();

                        npcDialogText.text = "[Take Rewards]";
                        option1Button.onClick.RemoveAllListeners();
                        option1Button.onClick.AddListener(() =>
                        {
                            ReceiveRewardAndCompleteQuest();
                        });
                    }
                    else
                    {
                        canvasUI.OpenDialogUI();

                        npcDialogText.text = currentActiveQuest.questInfoSO.comebackInProgress;
                        option1ButtonText.text = "Close";
                        option1Button.onClick.RemoveAllListeners();
                        option1Button.onClick.AddListener(() =>
                        {
                            canvasUI.CloseDialogUI();
                            isTalking = false;
                        });
                    }
                }

                if(currentActiveQuest.isCompleted == true)
                {
                    canvasUI.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.questInfoSO.lastDialouge;
                    option1ButtonText.text = "Close";
                    option1Button.onClick.RemoveAllListeners();
                    option1Button.onClick.AddListener(() =>
                    {
                        canvasUI.CloseDialogUI();
                        isTalking = false;
                    });
                }

                if(currentActiveQuest.initialDialogCompleted == false)
                {
                    StartQuestInitialDialog();
                }
            }          
        }

        

        private bool CheckQuestCompleted()
        {
            var requiredItems = currentActiveQuest.questInfoSO.requiredItem_and_Amount;

            foreach (var requirement in requiredItems)
            {
                ItemDataSO requiredItem = requirement.Key;
                int requiredAmount = requirement.Value;

                int playerHasAmount = 0;

                // 먼저 stash에서 검색
                if (Inventory.Instance.stashDictionary.TryGetValue(requiredItem, out InventoryItem stashItem))
                {
                    playerHasAmount += stashItem.stackSize;
                }

                // 그리고 inventory에서도 검색
                if (Inventory.Instance.inventoryDictionary.TryGetValue(requiredItem, out InventoryItem inventoryItem))
                {
                    playerHasAmount += inventoryItem.stackSize;
                }

                if (playerHasAmount < requiredAmount)
                {
                    return false; // 이 아이템이 부족하므로 퀘스트 미완료
                }
            }

            return true; // 모든 조건을 만족
        }

        private void HandoverRequireItems()
        {
            var requiredItems = currentActiveQuest.questInfoSO.requiredItem_and_Amount;

            foreach (var pair in requiredItems)
            {
                ItemDataSO item = pair.Key;
                int requiredAmount = pair.Value;

                int removedCount = 0;

                while (removedCount < requiredAmount)
                {
                    // RemoveItem은 스택 하나씩 제거하므로 반복 호출
                    Inventory.Instance.RemoveItem(item);
                    removedCount++;
                }
            }

        }

        private void ReceiveRewardAndCompleteQuest()
        {
            currentActiveQuest.isCompleted = true;

            var rewardMoney = currentActiveQuest.questInfoSO.rewardMoney;
            PlayerManager.Instance.currency += rewardMoney;

            var rewardItems = currentActiveQuest.questInfoSO.rewardItems;

            foreach (var item in rewardItems)
            {
                if (item == null)
                    continue;

                Inventory.Instance.AddItem(item);
                activeQuestIndex++;
                Debug.Log($"보상 아이템 {item.itemName} 을(를) 인벤토리에 추가했습니다.");
            }

            // Ready Next Quset
            if(activeQuestIndex < quests.Count)
            {
                currentActiveQuest = quests[activeQuestIndex];
                currentDialog = 0;
                canvasUI.CloseDialogUI();
                isTalking = false;
            }
            else
            {
                canvasUI.CloseDialogUI();
                isTalking = false;
                Debug.Log("No More Quests for this NPC");
            }

        }

        private void StartQuestInitialDialog()
        {
            canvasUI.OpenDialogUI();

            npcDialogText.text = currentActiveQuest.questInfoSO.initialDialogue[currentDialog];
            option1ButtonText.text = "Next";
            option1Button.onClick.RemoveAllListeners();
            option1Button.onClick.AddListener(() =>
            {
                currentDialog++;
                CheckIfDialogFinished();
            });

            option2Button.gameObject.SetActive(false);
        }

        private void CheckIfDialogFinished()
        {
            if(currentDialog == currentActiveQuest.questInfoSO.initialDialogue.Count - 1)
            {
                npcDialogText.text = currentActiveQuest.questInfoSO.initialDialogue[currentDialog];

                currentActiveQuest.initialDialogCompleted = true;

                SetAcceptAndDeclineOptions();
            }
            else // The case if there are more dialogss
            {
                npcDialogText.text = currentActiveQuest.questInfoSO.initialDialogue[currentDialog];

                option1ButtonText.text = "Next";
                option1Button.onClick.RemoveAllListeners();
                option1Button.onClick.AddListener(() =>
                {
                    currentDialog++;
                    CheckIfDialogFinished();
                });
            }
        }

        private void AcceptedQuest()
        {
            currentActiveQuest.accepted = true;
            currentActiveQuest.declined = false;

            if(currentActiveQuest.hasRequirements == false)
            {
                npcDialogText.text = currentActiveQuest.questInfoSO.comebackFinished;
                option1ButtonText.text = "< Take Award >";
                option1Button.onClick.RemoveAllListeners();
                option1Button.onClick.AddListener(() =>
                {
                    ReceiveRewardAndCompleteQuest();
                });
                option2Button.gameObject.SetActive(false);
            }
            else
            {
                npcDialogText.text = currentActiveQuest.questInfoSO.acceptAnswer;
                CloseDialogUI_ForNPC();
            }
        }

        

        private void DeclinedQuest()
        {
            currentActiveQuest.declined = true;

            npcDialogText.text = currentActiveQuest.questInfoSO.declineAnswer;
            option1ButtonText.text = "Close";
            CloseDialogUI_ForNPC();
        }

        private void SetAcceptAndDeclineOptions()
        {
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

        private void CloseDialogUI_ForNPC()
        {
            option1ButtonText.text = "Close";
            option1Button.onClick.RemoveAllListeners();
            option1Button.onClick.AddListener(() =>
            {
                canvasUI.CloseDialogUI();
                isTalking = false;
            });
            option2Button.gameObject.SetActive(false);
        }

        private void LookAtPlayer()
        {
            PlayerCharacter player = playerManager.playerCharacter;
            Vector3 direction = player.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);

            var yRotation = transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerDetected = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerDetected = false;
            }
        }
    }
}
