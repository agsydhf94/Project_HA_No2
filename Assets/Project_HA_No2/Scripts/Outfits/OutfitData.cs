using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitData : MonoBehaviour
{
    public GameObject fullBodyPrefab;

    [System.Serializable]
    public class PartEntry
    {
        public string partSlotName;
        public GameObject partPrefab;
        public string attachBoneName;

        public Vector3 localPosition;
        public Vector3 localRotation;
        public Vector3 localScale;
    }

    public List<PartEntry> parts = new();

}
