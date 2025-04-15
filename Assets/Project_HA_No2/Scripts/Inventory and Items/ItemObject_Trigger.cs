using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ItemObject_Trigger : MonoBehaviour
    {       
        private ItemObject itemObject => GetComponentInParent<ItemObject>();
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerCharacter>() != null)
            {
                itemObject.PickUpItem();
            }
        }
    }
}
