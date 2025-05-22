using TMPro;
using UnityEngine;

namespace HA
{
    public class InteractionManager : SingletonBase<InteractionManager>
    {
        [SerializeField] public GameObject interactionInfoUI;
        private TMP_Text interactionText;
        private Camera mainCamera;

        private void Start()
        {
            interactionText = interactionInfoUI.GetComponent<TMP_Text>();
            mainCamera = Camera.main;
        }

        void Update()
        {
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = mainCamera.ScreenPointToRay(screenCenterPoint);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var targetTransform = hit.transform;
                IInteractable interactable = targetTransform.GetComponent<IInteractable>();

                NPC npc = interactable as NPC;
                if(npc && npc.isPlayerDetected)
                {
                    interactionText.text = "Talk To " + $"< {interactable.GetInteractableName()} >";
                    interactionInfoUI.SetActive(true); 

                    if(Input.GetKeyDown(KeyCode.F) && npc.isTalking == false)
                    {
                        npc.StartConversation();
                    }

                    
                }
                else
                {
                    interactionText.text = "";
                    interactionInfoUI.SetActive(false);
                }
            }
            else
            {
                interactionInfoUI.SetActive(false);
            }
        }
    }
}
