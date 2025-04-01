using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public interface IVFXInjection
    {
        public void Initialize(IVFXPlayable injectedVFXPlayer);
    }
}
