using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    [CreateAssetMenu(menuName = "Game/WeaponData")]
    public class WeaponDataSO : ScriptableObject
    {
        public string weaponName;
        public GameObject prefab;
        public WeaponType weaponType;
        public float cooldown;
        public int maxAmmo;
        public AudioClip fireSound;
    }
}
