using System.Text;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Material
}

namespace HA
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "DataSO/Item")]
    public class ItemDataSO : ScriptableObject
    {
        public ItemType itemType;
        public string itemName;
        public Sprite icon;


        [Range(0, 100)] public float dropChance;

        protected StringBuilder sb = new StringBuilder();

        public virtual string GetDescription()
        {
            return "";
        }
    }
}
