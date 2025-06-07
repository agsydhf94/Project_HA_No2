using UnityEngine;

namespace HA
{
    /// <summary>
    /// Interface that defines movement behavior for AI agents.
    /// This abstraction allows different movement implementations
    /// such as NavMesh-based, flying, or custom pathfinding.
    /// </summary>
    public interface IMovementStrategy
    {
        /// <summary>
        /// Commands the agent to move toward the given destination.
        /// </summary>
        /// <param name="destination">The target world position.</param>
        void MoveTo(Vector3 destination);

        /// <summary>
        /// Checks if the agent has reached the given destination.
        /// </summary>
        /// <param name="position">The position to compare with the agent's current location.</param>
        /// <returns>True if the agent is close enough to be considered as having arrived.</returns>
        bool ReachedAt(Vector3 position);

        /// <summary>
        /// Stops the agent's movement immediately.
        /// </summary>
        void Stop();

        /// <summary>
        /// Resumes movement toward the last commanded destination.
        /// </summary>
        void Resume();
    }
}
