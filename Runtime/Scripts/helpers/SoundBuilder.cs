using UnityEngine;
using Utilities.SoundService.Runtime.data;

namespace Utilities.SoundService.Runtime
{
    public class SoundBuilder
    {
        private readonly SoundManager _soundManager;
        private SoundData _soundData;
        private Vector3 _position = Vector3.zero;
        private bool _randomizePitch = false;

        public SoundBuilder(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }

        public SoundBuilder WithSoundData(SoundData soundData)
        {
            _soundData = soundData;
            return this;
        }

        public SoundBuilder WithPosition(Vector3 position)
        {
            _position = position;
            return this;
        }

        public SoundBuilder WithRandomPitch()
        {
            _randomizePitch = true;
            return this;
        }

        public void Play()
        {
            if (!_soundManager.CanPlaySound(_soundData))
            {
                return;
            }

            var soundEmitter = _soundManager.GetEmitter();
            soundEmitter.Initialize(_soundData, _soundManager);
            soundEmitter.transform.position = _position;
            soundEmitter.transform.parent = _soundManager.transform;

            if (_randomizePitch)
            {
                soundEmitter.RandomizePitch();
            }

            if (_soundData.IsFriquentSound)
            {
                _soundManager.FriquentSoundEmitters.Enqueue(soundEmitter);
            }

            soundEmitter.Play();
        }
    }
}