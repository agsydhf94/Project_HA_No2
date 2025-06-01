using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private ItemDataSO itemDataSO;
        private Inventory inventory;
        private CanvasUI canvasUI;

        private void Awake()
        {
            inventory = Inventory.Instance;
            canvasUI = CanvasUI.Instance;
        }

        private void SetUpItemName()
        {
            if (itemDataSO != null)
            {
                return;
            }

            gameObject.name = "Item Object : " + itemDataSO.itemName;
        }

        public void SetUpItem(ItemDataSO _itemDataSO, Vector3 _velocity)
        {
            itemDataSO = _itemDataSO;
            rb.velocity = _velocity;
        }

        public void PickUpItem()
        {
            if (!inventory.CanAddItemToInventory() && itemDataSO.itemType == ItemType.Equipment)
            {
                rb.velocity = new Vector3(0, 2, 0);
                return;                
            }

            canvasUI.GetIngameUI().itemPopupContainerUI.ShowItemPopup(itemDataSO);
            inventory.AddItem(itemDataSO);
            Destroy(gameObject);
        }
    }   
}
