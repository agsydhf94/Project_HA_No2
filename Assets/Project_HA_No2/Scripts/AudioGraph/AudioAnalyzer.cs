using UnityEngine;

namespace HA
{
    /// <summary>
    /// Analyzes audio spectrum data from an AudioSource using FFT.
    /// Provides real-time frequency band values for visualizations.
    /// </summary>
    public class AudioAnalyzer : MonoBehaviour
    {
        public AudioSource audioSource;
        public int spectrumSize = 512;
        public float[] spectrumData;

        void Start()
        {
            spectrumData = new float[spectrumSize];
        }

        void Update()
        {
            audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
        }

        public float GetBandValue(int bandIndex)
        {
            return spectrumData[bandIndex];
        }
    }
}
