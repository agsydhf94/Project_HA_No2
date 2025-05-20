using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace HA
{
    public class SoundManager : SingletonBase<SoundManager>
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioMixerGroup masterGroup;
        [SerializeField] private AudioMixerGroup sfxGroup;
        [SerializeField] private AudioMixerGroup bgmGroup;

        private Dictionary<string, SoundDataSO> soundLibrary = new();
        private List<SoundInstance> activeSounds = new();

        public override void Awake()
        {
            base.Awake();
            LoadAllSounds();
        }

        private async void LoadAllSounds()
        {
            var handle = Addressables.LoadAssetsAsync<SoundDataSO>("SoundDataSO", null);
            var sounds = await handle.Task;

            foreach (var sound in sounds)
            {
                if (!string.IsNullOrEmpty(sound.soundID))
                    soundLibrary[sound.soundID] = sound;
            }

            Addressables.Release(handle);
        }

        public void PlaySound(string soundID, Vector3? position = null)
        {
            if (!soundLibrary.TryGetValue(soundID, out var data))
            {
                Debug.LogWarning($"Sound ID '{soundID}' not found!");
                return;
            }

            GameObject host = new GameObject($"Sound_{soundID}");
            host.transform.position = position ?? Vector3.zero;

            var source = host.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = GetGroupFor(data.type);

            var instance = new SoundInstance(source, data);
            instance.Play();
            activeSounds.Add(instance);

            if (!data.loop)
                Destroy(host, data.clip.length);
        }

        public AudioMixerGroup GetGroupFor(SoundType type)
        {
            return type switch
            {
                SoundType.SFX => sfxGroup,
                SoundType.BGM => bgmGroup,
                _ => masterGroup,
            };
        }

        public void SetMasterVolume(float volume)
        {
            mixer.SetFloat("Master", Mathf.Log10(Mathf.Clamp01(volume)) * 20);
        }

        public void SetSFXVolume(float volume)
        {
            mixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp01(volume)) * 20);
        }

        public void SetBGMVolume(float volume)
        {
            mixer.SetFloat("BGM", Mathf.Log10(Mathf.Clamp01(volume)) * 20);
        }

        public void StopAll()
        {
            foreach (var sound in activeSounds)
            {
                sound.Stop();
                if (sound.source != null)
                    Destroy(sound.source.gameObject);
            }
            activeSounds.Clear();
        }

        public bool TryGetSoundData(string soundID, out SoundDataSO data)
        {
            return soundLibrary.TryGetValue(soundID, out data);
        }
    }
}
