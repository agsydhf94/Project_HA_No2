using Unity.Entities;
using Unity.Mathematics;

namespace HA
{
    /// <summary>
    /// A tag component used to request an explosion at a specific position with given force and radius.
    /// This is processed by the ExplosionSystem.
    /// </summary>
    public struct ExplosionRequest : IComponentData
    {
        public float3 position;
        public float radius;
        public float force;
    }
}
