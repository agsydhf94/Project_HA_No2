using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HA
{
    public class NavMeshMovement : MonoBehaviour, IMovementStrategy
    {
        private readonly NavMeshAgent agent;
        private Vector3 lastDestination;

        public NavMeshMovement(NavMeshAgent agent)
        {
            this.agent = agent;
            lastDestination = agent.transform.position;
        }

        public void MoveTo(Vector3 destination)
        {
            lastDestination = destination;
            agent.isStopped = false;
            agent.SetDestination(destination);
        }

        public void Stop()
        {
            agent.isStopped = true;
        }

        public void Resume()
        {
            agent.isStopped = false;
            agent.SetDestination(lastDestination);
        }
    }
}
