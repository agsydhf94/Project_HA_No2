using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;


namespace HA
{
    /// <summary>
    /// Custom Baker class that converts an authoring GameObject into an ECS entity with physics collider and transform components.
    /// Specifically handles BoxCollider conversion to Unity.Physics.BoxCollider.
    /// </summary>
    public class ObjectBaker : Baker<ObjectAuthoring>
    {
        /// <summary>
        /// Shared static collider to avoid recreating identical BoxColliders for each baked entity.
        /// </summary>
        private static BlobAssetReference<Collider> sharedCollider;

        /// <summary>
        /// Converts the authoring GameObject into an ECS entity by adding a PhysicsCollider and LocalTransform.
        /// If a BoxCollider is not present, the bake process logs an error and skips the conversion.
        /// </summary>
        /// <param name="authoring">The GameObject containing authoring components to convert.</param>
        public override void Bake(ObjectAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Renderable);

            var boxCollider = authoring.GetComponent<UnityEngine.BoxCollider>();
            if (boxCollider == null)
            {
                UnityEngine.Debug.LogError("BoxCollider is required.");
                return;
            }

            if (!sharedCollider.IsCreated)
            {
                var boxGeometry = new BoxGeometry
                {
                    Center = boxCollider.center,
                    Size = boxCollider.size,
                    Orientation = quaternion.identity,
                    BevelRadius = 0.01f
                };

                sharedCollider = Unity.Physics.BoxCollider.Create(
                    boxGeometry,
                    new CollisionFilter
                    {
                        BelongsTo = ~0u,
                        CollidesWith = ~0u,
                        GroupIndex = 0
                    }
                );
            }

            AddComponent(entity, new PhysicsCollider { Value = sharedCollider });

            AddComponent(entity, new LocalTransform
            {
                Position = authoring.transform.position,
                Rotation = authoring.transform.rotation,
                Scale = 1f
            });
        }
    }
}
