using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class Inventory : SingletonBase<Inventory>
    {
        public List<ItemDataSO> inventory = new List<ItemDataSO>();

        public void AddItem(ItemDataSO item)
        {
            inventory.Add(item);
        }
    }
}
