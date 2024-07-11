using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Utilities.SoundService.helpers;
using Utilities.SoundService.Runtime.data;


namespace Utilities.SoundService.Runtime
{
    public class SoundManager : MonoBehaviour
    {
        private IObjectPool<SoundEmitter> _soundEmitterPool;
        private readonly List<SoundEmitter> _activeSoundEmitters = new();
        private readonly helpers.ILogger _logger = new SoundServiceLogger();

        public readonly Queue<SoundEmitter> FrequentSoundEmitters = new();

        [SerializeField]
        private SoundEmitter _soundEmitterPrefab;

        [Header("Pool settings")]
        [SerializeField]
        private bool _collectionCheck = true;

        [SerializeField]
        private int _defaultPoolCapacity = 10;

        [SerializeField]
        private int _maxPoolSize = 100;

        [Tooltip("Maximum amount of sounds played at once")]
        [Header("Sound Settings")]
        [SerializeField]
        private int _maxSoundInstances = 30;

        public List<SoundEmitter> ActiveSoundEmitters => _activeSoundEmitters;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            InitializePool();
        }


        private void InitializePool()
        {
            _soundEmitterPool = new ObjectPool<SoundEmitter>
            (
                CreateSoundEmitter,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                _collectionCheck,
                _defaultPoolCapacity,
                _maxPoolSize
            );
        }

        private SoundEmitter CreateSoundEmitter()
        {
            var soundEmitter = Instantiate(_soundEmitterPrefab);
            soundEmitter.gameObject.SetActive(false);
            _logger.Log($"Sound emitter created: {soundEmitter.name}");

            return soundEmitter;
        }

        private void OnTakeFromPool(SoundEmitter soundEmitter)
        {
            soundEmitter.gameObject.SetActive(true);
            _activeSoundEmitters.Add(soundEmitter);
            _logger.Log($"Sound emitter taken from pool: {soundEmitter.name}");
        }

        private void OnReturnedToPool(SoundEmitter soundEmitter)
        {
            soundEmitter.gameObject.SetActive(false);
            _activeSoundEmitters.Remove(soundEmitter);
            _logger.Log($"Sound emitter returned to pool: {soundEmitter.name}");
        }

        private void OnDestroyPoolObject(SoundEmitter soundEmitter)
        {
            Destroy(soundEmitter.gameObject);
            _logger.Log($"Sound emitter destroyed: {soundEmitter.name}");
        }

        public SoundBuilder CreateSound() => new(this);

        public bool CanPlaySound(SoundData data)
        {
            if (data.IsFrequentSound == false)
                return true;
            if (FrequentSoundEmitters.Count < _maxSoundInstances ||
                !FrequentSoundEmitters.TryDequeue(out var emitter))
                return true;

            try
            {
                emitter.Stop();
                return true;
            }
            catch
            {
                _logger.Log("Emitter already released...");
            }

            return false;
        }


        public SoundEmitter GetEmitter()
        {
            _logger.Log("Getting sound emitter from pool...");
            var soundEmitter = _soundEmitterPool.Get();
            _logger.Log($"Sound emitters left in pool: {_soundEmitterPool.CountInactive}");
            return soundEmitter;
        }

        public void ReturnToPool(SoundEmitter soundEmitter)
        {
            _logger.Log("Releasing sound emitter back to pool...");
            _soundEmitterPool.Release(soundEmitter);
            _logger.Log($"Sound emitters left in pool: {_soundEmitterPool.CountInactive}");
        }
    }
}