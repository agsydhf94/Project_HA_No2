using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HA
{
    [Serializable]
    public class InventoryItem
    {
        public ItemDataSO itemDataSO;
        public int stackSize;

        public InventoryItem(ItemDataSO _itemDataSO)
        {
            itemDataSO = _itemDataSO;
            AddStack();
        }

        public void AddStack() => stackSize++;
        public void RemoveStack() => stackSize--;
    }
}

