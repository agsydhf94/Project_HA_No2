using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Applies a dissolve effect to the assigned renderers over time,
    /// typically used for visualizing enemy or object deaths.
    /// Supports material instancing, automatic setup, and async execution via UniTask.
    /// </summary>
    public class DissolveDeathEffect : MonoBehaviour, IDeathEffect
    {
        [Header("Target Renderers")]
        /// <summary>
        /// Target renderers to apply the dissolve effect to.
        /// If not manually assigned, SkinnedMeshRenderer or MeshRenderer will be automatically searched.
        /// </summary>
        [SerializeField] private Renderer[] renderers;

        [Header("Dissolve Settings")]
        /// <summary>
        /// Shader property name used for controlling the dissolve amount (usually "_Dissolve").
        /// </summary>
        [SerializeField] private string dissolveParam = "_Dissolve";

        /// <summary>
        /// Total time it takes to fully dissolve.
        /// </summary>
        [SerializeField] private float duration = 1.5f;

        /// <summary>
        /// If enabled, dissolve effect automatically plays on OnEnable().
        /// </summary>
        [SerializeField] private bool autoStartOnEnable = false;

        [Header("Optional Dissolve Material")]
        /// <summary>
        /// Optional material preset used to replace original materials with dissolvable versions.
        /// </summary>
        public Material dissolveMaterialPreset;

        /// <summary>
        /// Whether to automatically apply the preset material if available.
        /// </summary>
        [SerializeField] private bool usePresetMaterialIfAvailable = true;


        /// <summary>
        /// Event invoked after the dissolve animation completes.
        /// </summary>
        public Action onDissolveComplete;

        /// <summary>
        /// Instanced dissolve materials applied to the target renderers.
        /// </summary>
        public List<Material> materials { get; private set; } = new();

        private void Awake()
        {
            // Auto-detect renderers if not manually assigned
            if (renderers == null || renderers.Length == 0)
            {
                var skinned = GetComponentsInChildren<SkinnedMeshRenderer>();
                renderers = skinned.Length > 0 ? skinned : GetComponentsInChildren<MeshRenderer>();
            }

            if (renderers.Length == 0)
            {
                Debug.LogWarning($"[{nameof(DissolveDeathEffect)}] No Renderer found on {gameObject.name}");
                enabled = false;
                return;
            }

            if (usePresetMaterialIfAvailable && dissolveMaterialPreset != null)
            {
                ApplyDissolveMaterial(dissolveMaterialPreset);
            }
        }

        private void OnEnable()
        {
            if (autoStartOnEnable)
            {
                PlayAsync(gameObject).Forget();
            }   
        }

        /// <summary>
        /// Replaces all renderer materials with instanced copies of the provided dissolve material.
        /// Automatically resets the dissolve value to zero for each material.
        /// </summary>
        /// <param name="dissolveMaterial">The material to apply for the dissolve effect.</param>
        public void ApplyDissolveMaterial(Material dissolveMaterial)
        {
            if (dissolveMaterial == null)
            {
                Debug.LogError("[DissolveDeathEffect] Dissolve material is null.");
                return;
            }

            materials.Clear();

            foreach (var renderer in renderers)
            {
                Material[] newMats = new Material[renderer.materials.Length];

                for (int i = 0; i < newMats.Length; i++)
                {
                    var instancedMat = new Material(dissolveMaterial);
                    if (instancedMat.HasProperty(dissolveParam))
                        instancedMat.SetFloat(dissolveParam, 0f);

                    newMats[i] = instancedMat;
                    materials.Add(instancedMat);
                }

                renderer.materials = newMats;
            }
        }

        /// <summary>
        /// Begins the dissolve animation by gradually increasing the dissolve parameter on all materials.
        /// Once complete, invokes the onDissolveComplete event.
        /// </summary>
        /// <param name="target">Target GameObject (not used directly, provided for IDeathEffect compliance).</param>
        public async UniTask PlayAsync(GameObject target)
        {
            if (materials.Count == 0 && dissolveMaterialPreset != null)
                ApplyDissolveMaterial(dissolveMaterialPreset);

            if (materials.Count == 0)
            {
                Debug.LogWarning("[DissolveDeathEffect] No dissolve materials applied.");
                return;
            }

            float time = 0f;

            while (time < duration)
            {
                float t = time / duration;

                foreach (var mat in materials)
                {
                    if (mat.HasProperty(dissolveParam))
                        mat.SetFloat(dissolveParam, t);
                }

                time += Time.deltaTime;
                await UniTask.Yield();
            }

            foreach (var mat in materials)
            {
                if (mat.HasProperty(dissolveParam))
                    mat.SetFloat(dissolveParam, 1f);
            }

            onDissolveComplete?.Invoke();
        }
    }
}
