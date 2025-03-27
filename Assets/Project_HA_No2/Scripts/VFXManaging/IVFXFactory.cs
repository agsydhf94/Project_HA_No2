using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public interface IVFXFactory
    {
        Component LoadFX(string key);
    }
}
