using UnityEngine;
using Utilities.SoundService.Runtime;
using Utilities.SoundService.Runtime.data;

namespace Utilities
{
    public class SoundUserExample : MonoBehaviour
    {
        [SerializeField]
        private SoundData _soundData;

        [SerializeField]
        private SoundData _ambient;

        [SerializeField]
        private SoundManager _soundManager;

        [SerializeField]
        private float _fireRate;

        private double _lastFireTime;

        private void Start()
        {
            _soundManager.CreateSound()
                .WithSoundData(_ambient)
                .Play();
        }
        
        private void Update()
        {
            if (!Input.GetKey(KeyCode.Space)) 
                return;
            if (!(Time.time > _lastFireTime + _fireRate)) 
                return;
            
            _lastFireTime = Time.time;
            Fire();
        }

        private void Fire()
        {
            _soundManager.CreateSound()
                .WithSoundData(_soundData)
                .WithRandomPitch(new Vector2(-.05f,0.05f))
                .Play();
        }
    }
}