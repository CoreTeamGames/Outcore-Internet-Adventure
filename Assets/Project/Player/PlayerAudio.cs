using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OutcoreInternetAdventure.Player
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField] AudioSource source;

        [HideInInspector] public PlayerEvents events;

        public AudioClip dashMiss;
        public AudioClip slash;
        public AudioClip jump;
        public AudioClip land;
        public AudioClip hit;
        public AudioClip death;

        public void PlayCilp(AudioClip _clip)
        {
            source.PlayOneShot(_clip);
        }
    }
}