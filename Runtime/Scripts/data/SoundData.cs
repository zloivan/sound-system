using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Utilities.SoundService.Runtime.data
{
    [Serializable]
    public class SoundData
    {
        public AudioClip Clip;
        public AudioMixerGroup MixerGroup;
        public bool IsLooping = false;
        public bool IsFrequentSound = false;
        public float FadeIn = 0f;
        public float FadeOut = 0f;
    }
}