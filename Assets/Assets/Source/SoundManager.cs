using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Assets.Source
{
    internal class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField] AudioSource _levelComplete;
        [SerializeField] AudioSource _hookThrow;
        [SerializeField] AudioSource _wallHit;
        [SerializeField] AudioSource _move;
        [SerializeField] AudioSource _goldCollect;
        [SerializeField] AudioSource _timerIncreased;
        [SerializeField] AudioSource _characterTalk;
        [SerializeField] AudioSource _obstacleHit;
        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
        }
        private void PlayPitchedSequenced(AudioSource template, float rangeMin = 1f, float rangeMax = 1.1f)
        {
            // Create a new AudioSource and copy settings from the template
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = template.clip;
            newSource.volume = template.volume;
            newSource.pitch = UnityEngine.Random.Range(rangeMin, rangeMax);
            newSource.loop = template.loop;
            newSource.spatialBlend = template.spatialBlend;
            newSource.outputAudioMixerGroup = template.outputAudioMixerGroup;

            newSource.Play();
            Destroy(newSource, newSource.clip.length); // Destroy after the clip finishes
        }
        private void PlayPitched(AudioSource sound, float rangeMin = 1f, float rangeMax = 1.1f)
        {
            sound.pitch = UnityEngine.Random.Range(rangeMin, rangeMax);
            sound.Play();
        }
        public void LevelComplete()
        {
            PlayPitched(_levelComplete);
        }

        public void HookThrow()
        {
            PlayPitched(_hookThrow, 0.6f, 0.7f);
        }
        public void WallHit()
        {
            PlayPitchedSequenced(_wallHit, 0.8f, 0.9f);
        }

        public void ObstacleHit()
        {
            PlayPitched(_obstacleHit, 0.8f, 0.9f);
        }
        public void Move()
        {
            PlayPitchedSequenced(_move);
        }
        public void GoldCollect() 
        {
            PlayPitched(_goldCollect);
        }
        public void TimerIncrease()
        {
            PlayPitched(_timerIncreased, 0.8f, 0.9f);
        }

        public void CharacterTalk()
        {
            PlayPitched(_characterTalk, 0.5f, 0.6f);
        }
    }
}
