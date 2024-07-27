using IKhom.SoundSystem.Runtime.components;
using IKhom.SoundSystem.Runtime.data;
using IKhom.SoundSystem.Runtime.helpers;
using UnityEngine;

namespace IKhom.SoundSystem.Runtime
{
    public class SoundBuilder
    {
        private readonly SoundManager _soundManager;
        private SoundData _soundData;
        private readonly Vector3 _position = Vector3.zero;
        private Vector2? _randomizePitch;
        private readonly helpers.ILogger _logger = new SoundServiceLogger();

        public SoundBuilder(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }

        public SoundBuilder WithSoundData(SoundData soundData)
        {
            _soundData = soundData;
            return this;
        }

        public SoundBuilder WithRandomPitch(Vector2? randomPitch = null)
        {
            _randomizePitch = randomPitch ?? new Vector2(-0.05f, 0.05f);
            return this;
        }

        public void Play()
        {
            if (_soundData == null)
            {
                _logger.LogError("Sound data is null when creating sound");
                return;
            }
            
            if (!_soundManager.CanPlaySound(_soundData))
            {
                return;
            }

            var soundEmitter = _soundManager.GetEmitter();
            soundEmitter.Initialize(_soundData, _soundManager);
            soundEmitter.transform.position = _position;
            soundEmitter.transform.parent = _soundManager.transform;

            if (_randomizePitch is not null)
            {
                soundEmitter.RandomizePitch(_randomizePitch.Value.x, _randomizePitch.Value.y);
            }

            if (_soundData.IsFrequentSound)
            {
                _soundManager.FrequentSoundEmitters.Enqueue(soundEmitter);
            }

            
            soundEmitter.Play();
        }
    }
}