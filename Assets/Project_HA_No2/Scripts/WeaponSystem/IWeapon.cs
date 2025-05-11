using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public interface IWeapon
    {
        void Equip(Transform hand);
        void Use();
        bool IsUsable();
    }
}
