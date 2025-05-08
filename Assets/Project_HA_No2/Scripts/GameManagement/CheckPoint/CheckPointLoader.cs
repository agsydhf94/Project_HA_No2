using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace HA
{
    public static class CheckPointLoader
    {
        /// <summary>
        /// Addressables로 CheckpointDataSO를 불러옵니다.
        /// </summary>
        public static async UniTask<CheckpointDataSO> LoadCheckpointByID(string checkpointID)
        {
            var handle = Addressables.LoadAssetAsync<CheckpointDataSO>(checkpointID);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }

            Debug.LogWarning($"CheckpointDataSO not found for ID: {checkpointID}");
            return null;
        }


        /// <summary>
        /// 플레이어를 체크포인트 위치로 이동시키고 회전 및 컷신 처리를 수행합니다.
        /// 씬 로드 이후에 호출되어야 합니다.
        /// </summary>
        public static async UniTask<bool> PlacePlayerAtCheckpoint(string checkpointID)
        {
            var checkpointData = await LoadCheckpointByID(checkpointID);
            if (checkpointData == null)
                return false;

            await SceneManager.LoadSceneAsync(checkpointData.sceneName);

            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                var characterController = player.GetComponent<CharacterController>();
                characterController.enabled = false;
                player.transform.position = checkpointData.spawnPosition;
                player.transform.eulerAngles = checkpointData.spawnRotation;
                characterController.enabled = true;
            }

            if (checkpointData.triggerCutscene)
            {
                // 예: CutsceneManager.Instance.Play(checkpointID);
            }

            return true;
        }
    }
}
