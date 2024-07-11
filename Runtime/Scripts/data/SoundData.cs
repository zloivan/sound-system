using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Utilities.SoundService.data
{
    [Serializable]
    public class SoundData
    {
        public AudioClip Clip;
        public AudioMixerGroup MixerGroup;
        public bool IsLooping = false;
        public bool IsFrequentSound = false;
    }
}