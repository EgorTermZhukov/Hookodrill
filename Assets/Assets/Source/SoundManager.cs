using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        private void Awake()
        {
            
            if(Instance == null)
                Instance = this;
            else
            {
                Destroy(this);
            }
        }

        private void PlayPitched(AudioSource sound, float rangeMin = 1f, float rangeMax = 1.1f)
        {
            sound.pitch = UnityEngine.Random.Range(1f, 1.1f);
            sound.Play();
        }
        public void LevelComplete()
        {
            PlayPitched(_levelComplete);
        }

        public void HookThrow()
        {
            PlayPitched(_hookThrow);
        }
        public void WallHit()
        {
            PlayPitched(_wallHit);
        }
        public void Move()
        {
            PlayPitched(_move);
        }
        public void GoldCollect() 
        {
            PlayPitched(_goldCollect);
        }
        public void TimerIncrease()
        {
            PlayPitched(_timerIncreased, 0.6f, 0.7f);
        }
    }
}
