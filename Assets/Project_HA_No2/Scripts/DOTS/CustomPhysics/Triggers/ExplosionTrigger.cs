using Unity.Entities;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Triggers an explosion by creating an entity with <see cref="ExplosionRequest"/> component in the ECS world.
    /// 
    /// This MonoBehaviour bridges traditional Unity code with ECS-based explosion systems.
    /// </summary>
    public class ExplosionTrigger : MonoBehaviour
    {
        /// <summary>
        /// The radius of the explosion area to affect nearby entities.
        /// </summary>
        public float radius;

        /// <summary>
        /// The strength of the force applied to affected entities.
        /// </summary>
        public float force;


        /// <summary>
        /// Creates an ECS entity with <see cref="ExplosionRequest"/> to trigger an explosion at the given position.
        /// </summary>
        /// <param name="worldPosition">The world-space position where the explosion occurs.</param>
        public void TriggerExplosion(Vector3 worldPosition)
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var explosionEntity = entityManager.CreateEntity();

            entityManager.AddComponentData(explosionEntity, new ExplosionRequest
            {
                position = worldPosition,
                radius = radius,
                force = force
            });

            Debug.Log("Explosion triggered.");
        }
    }
}
