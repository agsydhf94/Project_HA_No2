using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace HA
{
    /// <summary>
    /// Manages scene transitions with optional pre-save. Ensures only one transition at a time.
    /// </summary>
    public class SceneTransitionManager : SingletonBase<SceneTransitionManager>
    {
        /// <summary>
        /// Prevents multiple simultaneous transitions.
        /// </summary>
        private bool isTransitioning = false;

        /// <summary>
        /// Loads a scene asynchronously, optionally saving game state beforehand.
        /// </summary>
        /// <param name="sceneName">Scene name to load.</param>
        /// <param name="saveBefore">If true, saves game state before loading.</param>
        public async UniTask LoadSceneAsync(string sceneName, bool saveBefore = false)
        {
            if (isTransitioning) return;
            isTransitioning = true;

            if (saveBefore)
            {
                SaveManager.Instance?.SaveGame();
            }

            await SceneManager.LoadSceneAsync(sceneName);
            // Wait one frame after loading to allow objects to initialize
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);

            isTransitioning = false;
        }
    }
}
