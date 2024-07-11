using System.Collections;
using UnityEngine;

namespace Utilities.SoundService.Runtime.data
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        public SoundData Data { get; private set; }
        private AudioSource _audioSource;
        private Coroutine _playingCoroutine;
        private Coroutine _fadeCoroutine;
        private SoundManager _soundManager;

        private void Awake()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
        }

        private IEnumerator WaitForSoundToEnd()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying);
            _soundManager.ReturnToPool(this);
        }

        private void StartFadeIn(float duration)
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }

            _audioSource.volume = 0f;
            _audioSource.Play();
            _fadeCoroutine = StartCoroutine(FadeCoroutine(0f, 1f, duration));
        }

        private void StartFadeOut(float duration)
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }

            _fadeCoroutine = StartCoroutine(FadeCoroutine(1f, 0f, duration, true));
        }

        private IEnumerator FadeCoroutine(float startVolume,
            float endVolume,
            float duration,
            bool stopAfterFade = false)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var newVolume = Mathf.Lerp(startVolume, endVolume, elapsedTime / duration);
                _audioSource.volume = newVolume;
                yield return null;
            }

            _audioSource.volume = endVolume;

            if (stopAfterFade)
            {
                _audioSource.Stop();
                _soundManager.ReturnToPool(this);
            }

            _fadeCoroutine = null;
        }

        public void Play(float fadeInOverride = 0)
        {
            var fadeDuration = fadeInOverride > 0 
                ? fadeInOverride : Data.FadeIn > 0 
                    ? Data.FadeIn : 0;

            if (_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
            }

            if (fadeDuration > 0)
            {
                StartFadeIn(Data.FadeIn);
            }
            else
            {
                _audioSource.Play();
            }

            _playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        public void Stop(float fadeOutOverride = 0)
        {
            var fadeDuration = fadeOutOverride > 0 
                ? fadeOutOverride : Data.FadeOut > 0 
                    ? Data.FadeOut : 0;
            
            if (_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
                _playingCoroutine = null;
            }

            if (fadeDuration > 0)
            {
                StartFadeOut(Data.FadeOut);
            }
            else
            {
                _audioSource.Stop();
                _soundManager.ReturnToPool(this);
            }
        }

        public void Initialize(SoundData data, SoundManager manager)
        {
            Data = data;
            _soundManager = manager;

            _audioSource.clip = Data.Clip;
            _audioSource.outputAudioMixerGroup = Data.MixerGroup;
            _audioSource.loop = Data.IsLooping;
        }

        public void RandomizePitch(float min = -0.05f, float max = 0.05f)
        {
            _audioSource.pitch += Random.Range(min, max);
        }
    }
}