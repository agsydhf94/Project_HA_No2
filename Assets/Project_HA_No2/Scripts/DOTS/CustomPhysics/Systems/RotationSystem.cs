using Unity.Burst;
using Unity.Entities;

namespace HA
{
    /// <summary>
    /// A system that schedules the AngularRotationJob to update entity rotations.
    /// 
    /// It runs every frame and applies angular motion based on AngularVelocity component.
    /// </summary>
    [BurstCompile]
    public partial struct RotationSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new AngularRotationJob
            {
                deltaTime = SystemAPI.Time.DeltaTime
            };

            job.ScheduleParallel();
        }
    }
}
