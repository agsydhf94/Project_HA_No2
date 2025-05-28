#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.IO;


namespace HA
{
    public static class AddressablesKeyAssigner
    {
        [MenuItem("Tools/Addressables/Auto Assign Prefab Keys")]
        public static void AutoAssignPrefabKeys()
        {
            string prefabRoot = "Assets/AddressableAssets"; // �ּ� ���� ��Ʈ
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabRoot });

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("AddressableAssetSettings not found. Please create Addressables Settings via 'Window > Asset Management > Addressables > Groups'");
                return;
            }

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                // Ű ����: "Enemies/Orc.prefab" => "Enemies/Orc"
                string key = GenerateKey(assetPath, prefabRoot);
                if (string.IsNullOrEmpty(key)) continue;

                var entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
                entry.address = key;

                Debug.Log($"[Addressables] Assigned key '{key}' to: {assetPath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[Addressables] Prefab key assignment complete.");
        }

        private static string GenerateKey(string assetPath, string rootPath)
        {
            // ��Ʈ ���� ��� ��� ����
            string relativePath = assetPath.Replace(rootPath + "/", "");
            if (!relativePath.EndsWith(".prefab")) return null;

            string key = Path.ChangeExtension(relativePath, null); // ".prefab" ����
            return key.Replace("\\", "/"); // Windows ����
        }
    }
}
#endif
