using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public interface IObjectFactory
    {
        Component CreateObject(string key, Vector3 position, Quaternion rotation, Transform parent = null);
    }
}
