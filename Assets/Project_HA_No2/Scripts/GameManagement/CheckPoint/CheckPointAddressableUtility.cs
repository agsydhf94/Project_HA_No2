using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace HA
{
    public static class CheckPointAddressableUtility
    {
        [MenuItem("Tools/Addressables/Auto Assign Checkpoint Keys")]
        public static void AutoAssignCheckpointKeys()
        {
            string[] guids = AssetDatabase.FindAssets("t:CheckpointDataSO");
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<CheckpointDataSO>(path);
                if (asset == null) continue;

                var entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
                entry.address = asset.checkpointID;
                Debug.Log($"Assigned Addressables key: {asset.checkpointID} for {path}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}