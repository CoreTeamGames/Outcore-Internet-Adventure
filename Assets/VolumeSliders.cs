using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSliders : MonoBehaviour
{
    [Serializable]
    public class VolumeClass
    {
        public AudioMixerGroup audioMixerGroup;
        public Slider slider;
    }
    [SerializeField] List<VolumeClass> _volumes;

    public void Start() => SetVolumeToAllMixers();

    public void SetVolumeToAllMixers()
    {
        foreach (var item in _volumes)
        {
            item.audioMixerGroup.audioMixer.SetFloat(item.audioMixerGroup.name + "Volume", item.slider.value);
        }
    }
}
