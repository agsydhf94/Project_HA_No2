using Unity.Burst;
using Unity.Entities;

namespace HA
{
    /// <summary>
    /// A system that schedules a movement job for all entities with CustomVelocity.
    /// 
    /// It updates positions based on their velocity each frame using deltaTime.
    /// </summary>
    [BurstCompile]
    public partial struct MovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new MovementJob
            {
                deltaTime = SystemAPI.Time.DeltaTime
            };

            state.Dependency = job.ScheduleParallel(state.Dependency);
        }
    }
}
