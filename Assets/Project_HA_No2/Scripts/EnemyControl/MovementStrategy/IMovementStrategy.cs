using UnityEngine;

namespace HA
{
    public interface IMovementStrategy
    {
        void MoveTo(Vector3 destination);
        void Stop();
        void Resume();
    }
}
