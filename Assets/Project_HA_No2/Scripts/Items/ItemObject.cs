using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private ItemDataSO itemDataSO;

        private void OnValidate()
        {
            gameObject.name = "Item Object : " +itemDataSO.itemName;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<PlayerCharacter>() != null)
            {
                Inventory.Instance.AddItem(itemDataSO);
                Destroy(gameObject);
            }
        }
    }   
}
