using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HA
{
    public class AudioManager : SingletonBase<AudioManager>
    {
        [Header("Mixer References")]
        public AudioMixer masterMixer;

        [Header("AudioSources")]
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Default Settings")]
        public bool enableBGM = true;
        public bool enableSFX = true;

        private Dictionary<string, AudioClip> clipCache = new();

        #region BGM
        public async UniTask PlayBGM(string key, float fadeDuration = 1f)
        {
            if (!enableBGM) return;

            var clip = await LoadClipAsync(key);
            if (clip == null) return;

            if (bgmSource.isPlaying)
                await FadeOut(bgmSource, fadeDuration);

            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();

            await FadeIn(bgmSource, fadeDuration);
        }

        public void StopBGM() => bgmSource.Stop();
        #endregion

        #region SFX
        public async UniTask PlaySFX(string key)
        {
            if (!enableSFX) return;

            var clip = await LoadClipAsync(key);
            if (clip == null) return;

            sfxSource.PlayOneShot(clip);
        }

        public async UniTask PlaySFXAtPosition(string key, Vector3 position)
        {
            if (!enableSFX) return;

            var clip = await LoadClipAsync(key);
            if (clip == null) return;

            AudioSource.PlayClipAtPoint(clip, position);
        }

        public async UniTask PlaySFX3D(string key, Transform parent, float spatialBlend = 1f)
        {
            if (!enableSFX) return;

            var clip = await LoadClipAsync(key);
            if (clip == null) return;

            var source = parent.gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = spatialBlend;
            source.minDistance = 1f;
            source.maxDistance = 20f;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            source.Play();

            Destroy(source, clip.length + 0.1f);
        }
        #endregion

        #region Utility
        private async UniTask<AudioClip> LoadClipAsync(string key)
        {
            if (clipCache.TryGetValue(key, out var cachedClip))
                return cachedClip;

            var handle = Addressables.LoadAssetAsync<AudioClip>(key);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                clipCache[key] = handle.Result;
                return handle.Result;
            }

            Debug.LogWarning($"AudioClip not found: {key}");
            return null;
        }

        private async UniTask FadeOut(AudioSource source, float duration)
        {
            float startVolume = source.volume;
            float time = 0f;
            while (time < duration)
            {
                source.volume = Mathf.Lerp(startVolume, 0f, time / duration);
                time += Time.deltaTime;
                await UniTask.Yield();
            }
            source.volume = 0f;
            source.Stop();
            source.volume = startVolume;
        }

        private async UniTask FadeIn(AudioSource source, float duration)
        {
            float targetVolume = 1f;
            float time = 0f;
            source.volume = 0f;
            while (time < duration)
            {
                source.volume = Mathf.Lerp(0f, targetVolume, time / duration);
                time += Time.deltaTime;
                await UniTask.Yield();
            }
            source.volume = targetVolume;
        }
        #endregion
    }
}
