using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


namespace HA
{
    /// <summary>
    /// A DOTS system that processes queued explosion requests.
    /// For each ExplosionRequest, it:
    /// - Applies gravity to nearby entities if not already present
    /// - Adds AddImpulse and AngularVelocity components to simulate explosion effects
    /// - Schedules a parallel job (ExplosionJob) to apply directional impulse based on distance from explosion center
    /// - Destroys the ExplosionRequest entity after processing
    /// </summary>
    [BurstCompile]
    public partial struct ExplosionSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (request, entity) in SystemAPI.Query<RefRO<ExplosionRequest>>().WithEntityAccess())
            {
                var explosionPos = request.ValueRO.position;
                var radius = request.ValueRO.radius;
                var force = request.ValueRO.force;

                // Activate gravity and impulse only once per affected entity
                foreach (var (transform, targetEntity) in
                    SystemAPI.Query<RefRO<LocalTransform>>().WithEntityAccess())
                {
                    float3 dir = transform.ValueRO.Position - explosionPos;
                    float dist = math.length(dir);
                    if (dist > radius) continue;

                    // Add gravity if missing
                    if (!SystemAPI.HasComponent<CustomGravity>(targetEntity))
                    {
                        ecb.AddComponent(targetEntity, new CustomGravity
                        {
                            gravity = new float3(0, -9.81f, 0)
                        });
                    }

                    // Add impulse marker if missing
                    if (!SystemAPI.HasComponent<AddImpulse>(targetEntity))
                    {
                        ecb.AddComponent(targetEntity, new AddImpulse
                        {
                            value = float3.zero
                        });
                    }

                    // Add angular velocity if missing
                    if (!SystemAPI.HasComponent<AngularVelocity>(targetEntity))
                    {
                        float3 randomSpin = new float3(
                            UnityEngine.Random.Range(-5f, 5f),
                            UnityEngine.Random.Range(-5f, 5f),
                            UnityEngine.Random.Range(-5f, 5f)
                        );
                        ecb.AddComponent(targetEntity, new AngularVelocity
                        {
                            angular = randomSpin
                        });
                    }
                }

                // Schedule explosion force application job
                var job = new ExplosionJob
                {
                    explosionPos = explosionPos,
                    radius = radius,
                    force = force
                };
                state.Dependency = job.ScheduleParallel(state.Dependency);

                // One-shot request, remove after processing
                ecb.DestroyEntity(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
