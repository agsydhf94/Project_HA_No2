using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HA
{
    /// <summary>
    /// Utility class for accessing ScriptableRendererFeatures in URP at runtime.
    /// 
    /// Allows retrieval of specific render features from the default renderer (index 0)
    /// via reflection. Useful for dynamically enabling/disabling custom passes or effects.
    /// </summary>
    public static class RendererController
    {
        /// <summary>
        /// Retrieves a renderer feature of type T from the default URP renderer.
        /// </summary>
        /// <typeparam name="T">The type of ScriptableRendererFeature to retrieve.</typeparam>
        /// <returns>
        /// The matching renderer feature if found; otherwise, null.
        /// </returns>
        public static T GetFeature<T>() where T : ScriptableRendererFeature
        {
            var urp = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            if (urp == null)
            {
                Debug.LogWarning("[RendererController] Current pipeline is not URP.");
                return null;
            }

            // Access the default renderer (index 0)
            var renderer = urp.GetRenderer(0);
            if (renderer == null)
            {
                Debug.LogWarning("[RendererController] Default renderer not found.");
                return null;
            }


            // Use reflection to access the internal 'rendererFeatures' list
            var property = typeof(ScriptableRenderer).GetProperty(
                "rendererFeatures",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            if (property == null)
            {
                Debug.LogError("[RendererController] Unable to access 'rendererFeatures' property.");
                return null;
            }

            var features = property.GetValue(renderer) as List<ScriptableRendererFeature>;
            if (features == null)
            {
                Debug.LogWarning("[RendererController] Renderer feature list is null.");
                return null;
            }

            // Search for a matching feature of type T
            foreach (var feature in features)
            {
                if (feature is T matched)
                    return matched;
            }

            Debug.LogWarning($"[RendererController] Feature of type {typeof(T).Name} not found.");
            return null;
        }
    }
}
