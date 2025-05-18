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
            AddressableAssetUtility.AutoAssignKeys<CheckpointDataSO>(
                asset => asset.checkpointID
            );
        }
    }
}