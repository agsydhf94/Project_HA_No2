using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private ItemDataSO itemDataSO;


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
            Inventory.Instance.AddItem(itemDataSO);
            Destroy(gameObject);
        }
    }   
}
