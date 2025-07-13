using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Arranges visualizer bars in a circular pattern around the parent object.
    /// Each bar is instantiated and rotated to face the center.
    /// </summary>
    public class CircularVisualizer : MonoBehaviour
    {
        public GameObject barPrefab;
        public int barCount = 64;
        public float radius = 5f;

        private Transform[] bars;

        void Awake()
        {
            bars = new Transform[barCount];
            float angleStep = 360f / barCount;

            for (int i = 0; i < barCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

                GameObject bar = Instantiate(barPrefab, transform);
                bar.transform.localPosition = pos;
                bar.transform.LookAt(transform.position);
                bars[i] = bar.transform;
            }
        }

        public Transform[] GetBars() => bars;
    }
}
