using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public static class FindClosestEnemy
    {
        public static Transform GetClosestEnemy(Transform self, bool excludeSelf = false)
        {
            List<Collider> coliiders = ObjectDetection.GetObjectsBy<Enemy>(self, 5f);

            float closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;

            foreach (var collider in coliiders)
            {
                if (excludeSelf && collider.transform == self)
                    continue;

                float distanceToEnemy = Vector3.Distance(self.position, collider.transform.position);

                if (distanceToEnemy < closestDistance)
                    closestDistance = distanceToEnemy;
                closestEnemy = collider.transform;
            }

            if(closestEnemy != null)
            {
                return closestEnemy;
            }
            else
            {
                return null;
            }
            
        }
    }
}
