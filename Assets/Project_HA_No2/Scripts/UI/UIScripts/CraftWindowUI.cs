using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    public class CraftWindowUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private TMP_Text itemDescription;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Button craftButton;

        [SerializeField] private Image[] materialImage;

        public void SetUpCraftWindow(EquipmentDataSO data)
        {
            craftButton.onClick.RemoveAllListeners();

            for (int i = 0; i < materialImage.Length; i++)
            {
                materialImage[i].color = Color.clear;
                materialImage[i].GetComponentInChildren<TMP_Text>().color = Color.clear;     
            }

            for (int i = 0; i < data.requirementsForCraft.Count; i++)
            {
                if(data.requirementsForCraft.Count > materialImage.Length)
                {
                    
                }

                materialImage[i].sprite = data.requirementsForCraft[i].itemDataSO.icon;
                materialImage[i].color = Color.white;

                TMP_Text materialSlotText = materialImage[i].GetComponentInChildren<TMP_Text>();
                materialSlotText.text = data.requirementsForCraft[i].stackSize.ToString();
                materialSlotText.color = Color.white;

            }

            itemIcon.sprite = data.icon;
            itemName.text = data.itemName;
            itemDescription.text = data.GetDescription();

            craftButton.onClick.AddListener(() => Inventory.Instance.CanCraft(data, data.requirementsForCraft));
        }
    }
}
