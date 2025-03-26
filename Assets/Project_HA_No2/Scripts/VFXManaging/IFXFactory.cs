using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public interface IFXFactory
    {
        Component CreateFX(string key);
    }
}
