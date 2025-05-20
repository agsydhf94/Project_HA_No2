using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    public enum SoundType { SFX, BGM }

    [CreateAssetMenu(fileName = "SoundData", menuName = "DataSO/Sound Data")]
    public class SoundDataSO : ScriptableObject
    {
        public string soundID;
        public AudioClip clip;
        public SoundType type;
        public bool loop;
        public float volume = 1.0f;
    }
}
