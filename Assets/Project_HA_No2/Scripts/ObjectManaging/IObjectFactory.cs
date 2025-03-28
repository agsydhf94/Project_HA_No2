using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public interface IObjectFactory
    {
        Component LoadObject(string key);
    }
}
