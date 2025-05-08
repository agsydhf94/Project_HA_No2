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
        /// Addressables�� CheckpointDataSO�� �ҷ��ɴϴ�.
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
        /// �÷��̾ üũ����Ʈ ��ġ�� �̵���Ű�� ȸ�� �� �ƽ� ó���� �����մϴ�.
        /// �� �ε� ���Ŀ� ȣ��Ǿ�� �մϴ�.
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
                // ��: CutsceneManager.Instance.Play(checkpointID);
            }

            return true;
        }
    }
}
