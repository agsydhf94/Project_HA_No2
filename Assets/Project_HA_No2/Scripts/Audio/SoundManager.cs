using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace HA
{
    /// <summary>
    /// Centralized sound manager that handles playback of sound effects and background music.
    /// Loads sound data via Addressables and manages AudioMixer routing and volume control.
    /// </summary>
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

        /// <summary>
        /// Loads all sound assets labeled "SoundDataSO" from Addressables and stores them in a dictionary.
        /// </summary>
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


        /// <summary>
        /// Plays a sound using the given sound ID. Supports 3D positional sound and AudioSource injection.
        /// </summary>
        /// <param name="soundID">Unique ID for the sound to play.</param>
        /// <param name="position">World position for spatial sound playback (optional).</param>
        /// <param name="audioSource">Optional AudioSource to use instead of creating one.</param>
        public void PlaySound(string soundID, Vector3? position = null, AudioSource audioSource = null)
        {
            if (!soundLibrary.TryGetValue(soundID, out var data))
            {
                Debug.LogWarning($"Sound ID '{soundID}' not found!");
                return;
            }

            if (audioSource != null)
            {
                audioSource.outputAudioMixerGroup = GetGroupFor(data.type);
                audioSource.PlayOneShot(data.clip);
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


        /// <summary>
        /// Returns the appropriate AudioMixerGroup based on the sound type.
        /// </summary>
        /// <param name="type">The type of sound (e.g., SFX, BGM).</param>
        /// <returns>Corresponding AudioMixerGroup.</returns>
        public AudioMixerGroup GetGroupFor(SoundType type)
        {
            return type switch
            {
                SoundType.SFX => sfxGroup,
                SoundType.BGM => bgmGroup,
                _ => masterGroup,
            };
        }


        /// <summary>
        /// Sets the master volume using logarithmic scale.
        /// </summary>
        /// <param name="volume">Volume in the range [0, 1].</param>
        public void SetMasterVolume(float volume)
        {
            mixer.SetFloat("Master", Mathf.Log10(Mathf.Clamp01(volume)) * 20);
        }


        /// <summary>
        /// Sets the sound effects (SFX) volume using logarithmic scale.
        /// </summary>
        /// <param name="volume">Volume in the range [0, 1].</param>
        public void SetSFXVolume(float volume)
        {
            mixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp01(volume)) * 20);
        }


        /// <summary>
        /// Sets the background music (BGM) volume using logarithmic scale.
        /// </summary>
        /// <param name="volume">Volume in the range [0, 1].</param>
        public void SetBGMVolume(float volume)
        {
            mixer.SetFloat("BGM", Mathf.Log10(Mathf.Clamp01(volume)) * 20);
        }


        /// <summary>
        /// Stops and destroys all active sounds managed by the SoundManager.
        /// </summary>
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


        /// <summary>
        /// Attempts to retrieve the SoundDataSO associated with the given sound ID.
        /// </summary>
        /// <param name="soundID">The ID of the sound to look up.</param>
        /// <param name="data">Output parameter for the retrieved sound data.</param>
        /// <returns>True if found, false otherwise.</returns>
        public bool TryGetSoundData(string soundID, out SoundDataSO data)
        {
            return soundLibrary.TryGetValue(soundID, out data);
        }
    }
}
