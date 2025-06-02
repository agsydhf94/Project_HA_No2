using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HA
{
    public static class AddressablesHelper
    {
        public static async Task<Dictionary<string, T>> LoadAllSOByID<T>(string label, string idFieldName) where T : ScriptableObject
        {
            var dict = new Dictionary<string, T>();

        var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<T>(label, null);
            handle.Completed += (op) =>
            {
                Debug.Log($"로드된 아이템 개수: {op.Result.Count}");
            };
            await handle.Task;

        if (handle.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"[AddressablesHelper] Addressables.LoadAssetsAsync<{typeof(T).Name}> 실패. Label='{label}' 확인 필요.");
            return dict;
        }

        foreach (var asset in handle.Result)
        {
            var type = typeof(T);
            var field = type.GetField(idFieldName);
            if (field == null)
            {
                Debug.LogError($"[AddressablesHelper] '{type}'에 '{idFieldName}' 필드가 없습니다. 오타 또는 접근 제한 확인.");
                continue;
            }

            var id = field.GetValue(asset) as string;
            if (!string.IsNullOrEmpty(id))
                dict[id] = asset;
        }

        return dict;
        }
    }
}
