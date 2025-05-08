using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HA
{
    public class TitleUI : MonoBehaviour
    {
        [SerializeField] private string sceneName = "TitleScene";
        [SerializeField] private GameObject failedToLoadUI;
        [SerializeField] private FadeScreenUI fadeScreenUI;

        public void ContinueGame()
        {
            if(SaveManager.Instance.HasSavedData())
            {
                SaveManager.Instance.LoadGame();
                GameData gameData = SaveManager.Instance.gameData;
                string lastPassedCheckpointID = gameData.lastCheckpointID;

                ContinueGameAsync(lastPassedCheckpointID).Forget();
            }
            else
            {
                failedToLoadUI.SetActive(true);
            }
                
        }

        public void NewGame()
        {
            SaveManager.Instance.DeleteSaveData();
            StartCoroutine(ChanceSceneWithFadeEffect(1.5f));
            
        }

        public void ExitGame()
        {
            Debug.Log("Exit Game");
        }

        IEnumerator ChanceSceneWithFadeEffect(float delay)
        {
            fadeScreenUI.FadeOut();

            yield return new WaitForSeconds(delay);

            SceneManager.LoadScene(sceneName);
        }

        private async UniTaskVoid ContinueGameAsync(string checkpointID)
        {
            // Fade-in or Loading UI 시작
            fadeScreenUI.FadeOut(); // 예시

            await UniTask.Delay(1);

            await CheckPointLoader.PlacePlayerAtCheckpoint(checkpointID);
        }

    }
}
