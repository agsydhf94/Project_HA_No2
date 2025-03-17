using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class BallThrowSkill : Skill
    {
        [Header("Skill Information")]
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Vector3 throwDirection;
        [SerializeField] private float ballGravity;

    }
}
