using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    /// <summary>
    /// Controls the scaling and positioning of visualizer bars based on audio spectrum data.
    /// Supports multiple CircularVisualizers and applies mirrored EQ-based animation.
    /// </summary>
    public class VisualizerController : MonoBehaviour
    {
        public AudioAnalyzer analyzer;
        public List<CircularVisualizer> visualizers = new();
        public float heightMultiplier = 10f;     // Amplitude scaling factor
        public float smoothSpeed = 8f;           // Lerp speed for visual smoothing

        void Update()
        {
            foreach (var visualizer in visualizers)
            {
                var bars = visualizer.GetBars();
                int totalBars = bars.Length;
                int halfBarCount = totalBars / 2;

                int spectrumSize = analyzer.spectrumData.Length;
                int bandCount = halfBarCount;

                if (bandCount < 1 || spectrumSize < bandCount)
                    continue;

                // Calculate equalized frequency bands
                float[] equalizedBands = new float[bandCount];
                int samplesPerBand = spectrumSize / bandCount;

                for (int b = 0; b < bandCount; b++)
                {
                    float sum = 0f;
                    for (int s = 0; s < samplesPerBand; s++)
                    {
                        int index = b * samplesPerBand + s;
                        if (index < spectrumSize)
                            sum += analyzer.spectrumData[index];
                    }

                    float avg = sum / samplesPerBand;

                    // 보정: sqrt + log10
                    float value = Mathf.Pow(avg, 0.5f);
                    value = Mathf.Log10(value * 100f + 1e-6f);
                    value = Mathf.Clamp01(value);

                    equalizedBands[b] = value;
                }

                // Apply mirrored scaling to bars
                for (int i = 0; i < totalBars; i++)
                {
                    int mirroredIndex = i < halfBarCount ? i : (totalBars - 1 - i);
                    float value = equalizedBands[Mathf.Clamp(mirroredIndex, 0, equalizedBands.Length - 1)];

                    float targetZScale = value * heightMultiplier;
                    targetZScale = Mathf.Clamp(targetZScale, 0.01f, 3.5f); 

                    Transform barRoot = bars[i];
                    if (barRoot.childCount == 0) continue;

                    Transform barMesh = barRoot.GetChild(0);

                    Vector3 scale = barMesh.localScale;
                    scale.z = Mathf.Lerp(scale.z, targetZScale, Time.deltaTime * smoothSpeed);
                    barMesh.localScale = scale;

                    Vector3 pos = barMesh.localPosition;
                    pos.z = scale.z / 2f;
                    barMesh.localPosition = pos;
                }
            }
        }

    }
}
