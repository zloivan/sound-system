using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Utilities.SoundService.Runtime.data
{
    [Serializable]
    public struct SoundData
    {
        public AudioClip Clip;
        public AudioMixerGroup MixerGroup;
        public bool IsLooping;
        public bool IsFrequentSound;
    }
}