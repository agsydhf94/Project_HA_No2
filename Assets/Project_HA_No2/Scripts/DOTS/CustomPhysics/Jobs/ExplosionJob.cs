using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


namespace HA
{
    /// <summary>
    /// A parallel job that applies explosion impulse to entities within a specified radius.
    /// 
    /// For each entity with a CustomVelocity and AddImpulse component:
    /// - Computes direction and distance from the explosion center
    /// - Applies a velocity impulse away from the center, scaled by proximity to the explosion
    /// - Entities farther from the center receive less force
    /// </summary>
    [BurstCompile]
    public partial struct ExplosionJob : IJobEntity
    {
        public float3 explosionPos;
        public float radius;
        public float force;

        public void Execute(
            ref CustomVelocity velocity,
            in LocalTransform transform,
            in AddImpulse tag)
        {
            float3 dir = transform.Position - explosionPos;
            float dist = math.length(dir);
            if (dist > radius) return;

            float3 forceDir = math.normalizesafe(dir);
            float strength = force * (1f - dist / radius);
            velocity.linearVelocity += forceDir * strength;
        }
    }
}
