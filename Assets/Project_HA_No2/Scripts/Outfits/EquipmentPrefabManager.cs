using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HA
{

    public class EquipmentPrefabManager : MonoBehaviour
    {
        [Header("Bone Information")]
        public Transform skeletonRoot;
        public Transform[] characterBones;

        [Header("Attachment Root")]
        public Transform outfitRoot;

        private List<GameObject> currentParts = new();
        private Dictionary<string, List<GameObject>> equippedPartsBySlot = new();

        private void Awake()
        {
            AutoCollectBones();
        }

        public void ApplyPrefab(EquipmentDataSO outfitData)
        {
            ClearAllParts();

            foreach (var entry in outfitData.parts)
            {
                EquipPart(entry);
            }
        }


        public void EquipAllPartsWithSlot(string partSlotName, EquipmentDataSO outfitData)
        {
            var entries = outfitData.parts.Where(p => p.partSlotName == partSlotName);
            foreach (var entry in entries)
            {
                EquipPart(entry);
            }
        }


        public void EquipPart(EquipmentDataSO.PartEntry entry)
        {
            GameObject part = Instantiate(entry.partPrefab);
            AttachPart(part, entry);

            if (!equippedPartsBySlot.ContainsKey(entry.partSlotName))
                equippedPartsBySlot[entry.partSlotName] = new List<GameObject>();

            equippedPartsBySlot[entry.partSlotName].Add(part);
            currentParts.Add(part);
        }


        public void UnequipPart(EquipmentDataSO.PartEntry entry)
        {
            if (!equippedPartsBySlot.TryGetValue(entry.partSlotName, out var parts))
                return;

            var toRemove = parts.FirstOrDefault(part => part != null && part.name.StartsWith(entry.partPrefab.name));
            if (toRemove != null)
            {
                Destroy(toRemove);
                parts.Remove(toRemove);
                currentParts.Remove(toRemove);
            }

            if (parts.Count == 0)
                equippedPartsBySlot.Remove(entry.partSlotName);
        }


        public void UnequipParts(IEnumerable<EquipmentDataSO.PartEntry> entries)
        {
            foreach (var entry in entries)
            {
                UnequipPart(entry);
            }
        }


        public void ClearAllParts()
        {
            foreach (var part in currentParts)
            {
                if (part != null)
                    Destroy(part);
            }

            currentParts.Clear();
            equippedPartsBySlot.Clear();
        }


        private void AttachPart(GameObject part, EquipmentDataSO.PartEntry entry)
        {
            Transform parent = outfitRoot;

            if (!string.IsNullOrEmpty(entry.attachBoneName))
            {
                Transform bone = FindBone(entry.attachBoneName);
                if (bone != null)
                    parent = bone;
                else
                    Debug.LogWarning($"[OutfitComponent] Bone '{entry.attachBoneName}' not found. Using outfitRoot.");
            }

            part.transform.SetParent(parent, false);


            part.transform.localPosition = entry.localPosition;
            part.transform.localEulerAngles = entry.localRotation;
            part.transform.localScale = entry.localScale;

            BindSkinnedMesh(part);
        }


        private void BindSkinnedMesh(GameObject obj)
        {
            var smr = obj.GetComponentInChildren<SkinnedMeshRenderer>();
            if (smr != null && skeletonRoot != null && characterBones?.Length > 0)
            {
                smr.rootBone = skeletonRoot;
                smr.bones = characterBones;
            }
        }


        private Transform FindBone(string name)
        {
            foreach (var bone in characterBones)
            {
                if (bone.name == name)
                    return bone;
            }
            return null;
        }

        public void AutoCollectBones()
        {
            if (skeletonRoot != null)
            {
                characterBones = skeletonRoot.GetComponentsInChildren<Transform>();
            }
        }
    }
}
