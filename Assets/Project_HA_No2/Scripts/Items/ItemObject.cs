using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private ItemDataSO itemDataSO;

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<PlayerCharacter>() != null)
            {
                Debug.Log("������ ȹ��");
                Destroy(gameObject);
            }
        }
    }   
}
