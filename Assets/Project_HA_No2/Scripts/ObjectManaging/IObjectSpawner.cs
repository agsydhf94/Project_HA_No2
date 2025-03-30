using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public interface IObjectSpawner
    {
        void Spawn(Component component, Vector3 position, Quaternion rotation, Transform parent = null);
    }
}

