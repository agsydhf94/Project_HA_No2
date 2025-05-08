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
                StartCoroutine(LoadSceneWithFadeEffect(1.5f));
            }
            else
            {
                failedToLoadUI.SetActive(true);
            }
                
        }

        public void NewGame()
        {
            SaveManager.Instance.DeleteSaveData();
            StartCoroutine(LoadSceneWithFadeEffect(1.5f));
        }

        public void ExitGame()
        {
            Debug.Log("Exit Game");
        }

        IEnumerator LoadSceneWithFadeEffect(float delay)
        {
            fadeScreenUI.FadeOut();

            yield return new WaitForSeconds(delay);

            SceneManager.LoadScene(sceneName);
        }
    }
}
