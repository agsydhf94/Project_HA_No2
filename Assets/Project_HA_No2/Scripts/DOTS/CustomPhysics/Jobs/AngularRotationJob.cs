using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace HA
{
    /// <summary>
    /// A job that applies angular velocity to entities, updating their rotation over time.
    /// 
    /// It calculates the rotation delta from angular velocity and applies it to the LocalTransform's rotation.
    /// </summary>
    [BurstCompile]
    public partial struct AngularRotationJob : IJobEntity
    {
        public float deltaTime;

        public void Execute(ref LocalTransform transform, in AngularVelocity angularVelocity)
        {
            float3 angle = angularVelocity.angular * deltaTime;
            quaternion rot = quaternion.Euler(angle);
            transform.Rotation = math.normalize(math.mul(transform.Rotation, rot));
        }
    }
}
