using System.Collections;
using UnityEngine;
using Utilities.SoundService.data;

namespace Utilities.SoundService
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        public SoundData Data { get; private set; }

        private AudioSource _audioSource;
        private Coroutine _playingCoroutine;
        private SoundManager _soundManager;

        private void Awake()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
        }

        private IEnumerator WaitForSoundToEnd()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying); //TODO Probably cache the waitWhile class
            _soundManager.ReturnToPool(this);
        }

        public void Play()
        {
            if (_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
            }

            _audioSource.Play();
            _playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        public void Stop()
        {
            if (_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
                _playingCoroutine = null;
            }

            _audioSource.Stop();
            _soundManager.ReturnToPool(this);
        }

        public void Initialize(SoundData data, SoundManager manager)
        {
            Data = data;
            _audioSource.clip = Data.Clip;
            _audioSource.outputAudioMixerGroup = Data.MixerGroup;
            _audioSource.loop = Data.IsLooping;
            _soundManager = manager;
        }

        public void RandomizePitch(float min = -0.05f, float max = 0.05f)
        {
            _audioSource.pitch += Random.Range(min, max);
        }
    }
}