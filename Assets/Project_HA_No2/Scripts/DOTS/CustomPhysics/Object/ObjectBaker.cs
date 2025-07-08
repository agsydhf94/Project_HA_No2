using Unity.Entities;
using Unity.Mathematics;


namespace HA
{
    /// <summary>
    /// Custom Baker class that converts an authoring GameObject into an ECS entity with physics collider and transform components.
    /// Specifically handles BoxCollider conversion to Unity.Physics.BoxCollider.
    /// </summary>
    public class ObjectBaker : Baker<ObjectAuthoring>
    {
        public override void Bake(ObjectAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // Initialize custom velocity
            AddComponent(entity, new CustomVelocity
            {
                linearVelocity = (float3)authoring.initialVelocity
            });

            // Set custom gravity vector
            AddComponent(entity, new CustomGravity
            {
                gravity = (float3)authoring.gravity
            });

            // Add impulse receiver component if enabled
            if (authoring.receiveExplosionForce)
            {
                AddComponent<AddImpulse>(entity);
            }
        }
    }
}
