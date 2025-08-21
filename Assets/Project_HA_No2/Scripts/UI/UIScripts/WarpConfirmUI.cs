using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HA
{
    /// <summary>
    /// UI component that displays a confirmation dialog when the player interacts with a warp portal.
    /// Provides Yes/No buttons, a customizable message, and a fade-scaling effect.
    /// </summary>
    public class WarpConfirmUI : MonoBehaviour
    {
        /// <summary>
        /// Root panel group used for fading the confirmation dialog in and out.
        /// </summary>
        [SerializeField] private CanvasGroup panel;

        /// <summary>
        /// Button that confirms the warp action when clicked.
        /// </summary>
        [SerializeField] private Button yesButton;

        /// <summary>
        /// Button that cancels the warp action when clicked.
        /// </summary>
        [SerializeField] private Button noButton;

        /// <summary>
        /// Text element used to display the confirmation message to the player.
        /// </summary>
        [SerializeField] private TextMeshProUGUI message;

        /// <summary>
        /// Optional UI effect for fading and scaling the confirmation panel.
        /// </summary>
        [SerializeField] public UIFadeScaler uIFadeScaler;
    }
}
