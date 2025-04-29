using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HA
{
    public class CraftListUI : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Transform craftSlotParent;
        [SerializeField] private GameObject craftSlotPrefab;

        [SerializeField] private List<EquipmentDataSO> craftEquipment;

        private void Start()
        {
            transform.parent.GetChild(0).GetComponent<CraftListUI>().SetupCraftList();
            SetUpDefaultCraftWindow();
        }

        

        public void SetupCraftList()
        {
            for(int i = 0; i < craftSlotParent.childCount; i++)
            {
                Destroy(craftSlotParent.GetChild(i).gameObject);
            }

            

            for(int i = 0; i < craftEquipment.Count; i++)
            {
                GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
                newSlot.GetComponent<CraftSlotUI>().SetUpCraftSlot(craftEquipment[i]);
            }
        }

        public void SetUpDefaultCraftWindow()
        {
            if (craftEquipment[0] != null)
            {
                GetComponentInParent<CanvasUI>().craftWindowUI.SetUpCraftWindow(craftEquipment[0]);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetupCraftList();
        }
    }
}
