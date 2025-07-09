using Unity.Entities;
using Unity.Mathematics;

namespace HA
{
    public struct AngularVelocity : IComponentData
    {
        public float3 angular;
    }
}
