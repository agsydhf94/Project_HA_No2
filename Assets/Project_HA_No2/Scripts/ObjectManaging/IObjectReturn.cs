using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HA
{
    public interface IObjectReturn
    {
        event System.Action OnReturnRequested;
        void Return(string key, Component component); // 직접 요청 가능
    }
}
