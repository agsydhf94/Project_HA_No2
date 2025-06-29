using UnityEngine;

namespace HA
{
    /// <summary>
    /// Displays a temporary line using LineRenderer to visualize a bullet's path.
    /// The line is shown for a short duration and then automatically hidden.
    /// </summary>
    public class BulletLineRenderer : MonoBehaviour
    {
        private LineRenderer lineRenderer;

        /// <summary>
        /// How long the line should remain visible after being shown.
        /// </summary>
        public float displayTime = 0.05f;

        /// <summary>
        /// Internal timer to track how long the line has been visible.
        /// </summary>
        private float timer;


        /// <summary>
        /// Shows a line from the start position to the end position,
        /// representing the bullet's trajectory.
        /// </summary>
        /// <param name="start">The start point of the bullet.</param>
        /// <param name="end">The end point of the bullet.</param>
        public void Show(Vector3 start, Vector3 end)
        {
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            timer = displayTime;
        }

        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }


        /// <summary>
        /// Updates the timer and disables the line when the display time expires.
        /// </summary>
        void Update()
        {
            if (lineRenderer.enabled)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    lineRenderer.enabled = false;
                }
            }
        }
    }
}
