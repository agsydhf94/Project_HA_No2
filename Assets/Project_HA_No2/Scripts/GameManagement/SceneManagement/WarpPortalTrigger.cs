using UnityEngine;
using Cysharp.Threading.Tasks;

namespace HA
{
    /// <summary>
    /// Trigger volume that asks for confirmation and warps the player to a target scene.
    /// Optionally saves, carries a target spawn point ID via <see cref="WarpContext"/>,
    /// and places the player after the scene loads.
    /// </summary>
    public class WarpPortalTrigger : MonoBehaviour
    {
        [Header("Warp")]
        /// <summary>
        /// The name of the scene to load when the warp is confirmed.
        /// </summary>
        [SerializeField] private string targetSceneName;

        /// <summary>
        /// Optional spawn point ID in the destination scene.
        /// </summary>
        [SerializeField] private string targetSpawnPointId;

        /// <summary>
        /// Whether to save the game state before performing the warp.
        /// </summary>
        [SerializeField] private bool saveBeforeWarp = true;

        [Header("Misc")]
        /// <summary>
        /// Tag used to identify the player collider entering the trigger.
        /// </summary>
        [SerializeField] private string playerTag = "Player";

        /// <summary>
        /// Whether the player is currently inside the trigger volume.
        /// </summary>
        private bool playerInside;


        /// <summary>
        /// Opens the warp confirmation UI when the player enters the trigger.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(playerTag)) return;
            playerInside = true;
            CanvasUI.Instance.OpenWarpConfirmUI(OnConfirmYes, OnConfirmNo);
        }


        /// <summary>
        /// Closes the warp confirmation UI when the player exits the trigger.
        /// </summary>
        /// <param name="other">The collider that exited the trigger.</param>
        void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(playerTag)) return;
            playerInside = false;
            CanvasUI.Instance.CloseWarpConfirmUI();
        }


        /// <summary>
        /// Confirmation callback: performs the warp asynchronously.
        /// </summary>
        private void OnConfirmYes()
        {
            DoWarpAsync().Forget();
        }


        /// <summary>
        /// Confirmation callback: does nothing by default (reserved for future logic).
        /// </summary>
        private void OnConfirmNo()
        {
            // Intentionally left blank; extend if needed.
        }


        /// <summary>
        /// Executes the warp flow: set spawn context, load scene (with optional save),
        /// then place the player at the target spawn point.
        /// </summary>
        private async UniTask DoWarpAsync()
        {
            if (!string.IsNullOrEmpty(targetSpawnPointId))
                WarpContext.Set(targetSpawnPointId);

            PlayerSpawnPlacer.Instance.player = PlayerManager.Instance.playerCharacter.transform;
            await SceneTransitionManager.Instance.LoadSceneAsync(targetSceneName, saveBeforeWarp);
            PlayerSpawnPlacer.Instance.SetSpawnPlayer();
        }
    }
}
