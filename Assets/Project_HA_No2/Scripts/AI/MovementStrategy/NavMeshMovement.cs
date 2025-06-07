using UnityEngine;
using UnityEngine.AI;

namespace HA
{
    /// <summary>
    /// Movement strategy that uses Unity's NavMeshAgent for pathfinding.
    /// </summary>
    public class NavMeshMovement : MonoBehaviour, IMovementStrategy
    {
        private readonly NavMeshAgent agent;
        private Vector3 lastDestination;

        /// <summary>
        /// Initializes the movement strategy with a NavMeshAgent.
        /// </summary>
        /// <param name="agent">The NavMeshAgent component to control.</param>
        public NavMeshMovement(NavMeshAgent agent)
        {
            this.agent = agent;
            lastDestination = agent.transform.position;
        }

        /// <summary>
        /// Moves the agent to the specified destination.
        /// </summary>
        /// <param name="destination">World position to move to.</param>
        public void MoveTo(Vector3 destination)
        {
            lastDestination = destination;
            agent.isStopped = false;
            agent.SetDestination(destination);
        }

        /// <summary>
        /// Immediately stops the agent.
        /// </summary>
        public void Stop()
        {
            agent.isStopped = true;
        }

        /// <summary>
        /// Resumes movement toward the last known destination.
        /// </summary>
        public void Resume()
        {
            agent.isStopped = false;
            agent.SetDestination(lastDestination);
        }

        /// <summary>
        /// Returns true if the agent is close enough to the given position.
        /// </summary>
        /// <param name="position">The target position to compare with.</param>
        /// <returns>True if within stopping distance.</returns>
        public bool ReachedAt(Vector3 position)
        {
            float distance = Vector3.Distance(agent.transform.position, position);
            return distance <= agent.stoppingDistance + 0.1f;
        }
    }
}
