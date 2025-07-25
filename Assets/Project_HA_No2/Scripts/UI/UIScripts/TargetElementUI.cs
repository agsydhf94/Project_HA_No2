using UnityEngine;
using UnityEngine.UI;

namespace HA
{
    /// <summary>
    /// Manages a UI marker for a targetable object, allowing visual highlighting.
    /// Changes marker color based on highlight state.
    /// </summary>
    public class TargetElementUI : MonoBehaviour
    {
        /// <summary>
        /// The Image component used to visually represent the target marker.
        /// </summary>
        [SerializeField] private Image markerImage;

        /// <summary>
        /// The default color of the marker when not highlighted.
        /// </summary>
        [SerializeField] private Color defaultColor = new Color(1f, 1f, 1f, 0.5f);

        /// <summary>
        /// The color to use when the target is highlighted.
        /// </summary>
        [SerializeField] private Color highlightColor = Color.red;


        /// <summary>
        /// Tracks whether the target is currently highlighted.
        /// </summary>
        private bool isHighlighted = false;


        /// <summary>
        /// Initializes the marker to its default, non-highlighted state.
        /// </summary>
        public void Initialize()
        {
            SetHighlight(false);
        }


        /// <summary>
        /// Sets the highlight state of the marker and updates its color accordingly.
        /// </summary>
        /// <param name="highlight">If true, uses highlight color; otherwise, default color.</param>
        public void SetHighlight(bool highlight)
        {
            isHighlighted = highlight;
            markerImage.color = highlight ? highlightColor : defaultColor;
        }
    }
}
