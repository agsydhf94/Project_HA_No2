using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public class SoundInstance
    {
        public AudioSource source;
        public SoundDataSO data;

        public SoundInstance(AudioSource audioSource, SoundDataSO soundData)
        {
            source = audioSource;
            data = soundData;

            source.clip = data.clip;
            source.loop = data.loop;
            source.volume = data.volume;
            source.spatialBlend = (data.type == SoundType.SFX) ? 1.0f : 0.0f;
        }

        public void Play() => source.Play();
        public void Stop() => source.Stop();
        public bool IsPlaying => source.isPlaying;
    }
}
