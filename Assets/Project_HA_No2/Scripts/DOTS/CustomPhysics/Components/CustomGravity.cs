using Unity.Entities;
using Unity.Mathematics;

namespace HA
{
    public struct CustomGravity : IComponentData
    {
        public float3 gravity;
    }
}
