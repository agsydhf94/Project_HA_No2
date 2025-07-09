using Unity.Entities;
using Unity.Mathematics;


namespace HA
{
    public struct CustomVelocity : IComponentData
    {
        public float3 linearVelocity;
    }
}
