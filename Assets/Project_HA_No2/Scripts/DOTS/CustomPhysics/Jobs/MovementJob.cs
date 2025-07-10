using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace HA
{
    /// <summary>
    /// A job that updates the position of each entity based on its CustomVelocity.
    /// 
    /// Applies velocity * deltaTime to the LocalTransform.Position.
    /// </summary>
    [BurstCompile]
    public partial struct MovementJob : IJobEntity
    {
        public float deltaTime;

        public void Execute(ref LocalTransform transform, in CustomVelocity velocity)
        {
            transform.Position += velocity.linearVelocity * deltaTime;
        }
    }
}
