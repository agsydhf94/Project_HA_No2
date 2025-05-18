using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace HA
{
    public static class AddressableAssetUtility
    {
        public static void AutoAssignKeys<T>(System.Func<T, string> getKeyFunc) where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset == null) continue;

                string key = getKeyFunc(asset);
                if (string.IsNullOrEmpty(key)) continue;

                var entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
                entry.address = key;

                Debug.Log($"[Addressables] Assigned key '{key}' for {typeof(T).Name}: {path}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
