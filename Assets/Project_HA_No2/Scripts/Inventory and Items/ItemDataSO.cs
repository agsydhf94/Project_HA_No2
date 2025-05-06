using System.Text;
using UnityEditor;
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
        public string itemId;


        [Range(0, 100)] public float dropChance;

        protected StringBuilder sb = new StringBuilder();

        private void OnValidate()
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(this);
            itemId = AssetDatabase.AssetPathToGUID(path);
#endif
        }

        public virtual string GetDescription()
        {
            return "";
        }
    }
}
