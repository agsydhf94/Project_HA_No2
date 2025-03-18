using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class BallThrowSkill : Skill
    {
        [Header("Skill Information")]
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Transform ballPosition;
        [SerializeField] private Vector3 throwDirection;
        [SerializeField] private float ballGravity;

        private void Awake()
        {
            ballPosition = GameObject.FindWithTag("BallPosition").transform;
        }

        public void CreateBall()
        {
            GameObject ball = Instantiate(ballPrefab, ballPosition);
        }

    }
}
