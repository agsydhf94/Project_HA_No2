using UnityEngine;

namespace HA
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "DataSO/Item")]
    public class ItemDataSO : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
    }
}
