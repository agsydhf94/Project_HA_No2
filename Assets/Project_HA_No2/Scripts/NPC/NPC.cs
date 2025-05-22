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
        private PlayerManager playerManager;
        private CanvasUI canvasUI;

        public bool isPlayerDetected;
        public bool isTalking;
        public string npcName;

        private TMP_Text npcDialogText;

        private Button option1Button;
        private TMP_Text option1ButtonText;
        private Button option2Button;
        private TMP_Text option2ButtonText;


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
