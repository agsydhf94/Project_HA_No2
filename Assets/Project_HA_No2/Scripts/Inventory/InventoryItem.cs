using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class InventoryItem : MonoBehaviour
    {
        public ItemDataSO itemDataSO;
        public int stackSize;

        public InventoryItem(ItemDataSO _itemDataSO)
        {
            itemDataSO = _itemDataSO;
        }

        public void AddStack() => stackSize++;
        public void RemoveStack() => stackSize--;
    }
}

