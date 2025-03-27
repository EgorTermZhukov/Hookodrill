using System;
using System.Collections;
using Assets.Assets.Source;
using UnityEngine;

namespace Assets.Source
{
    public class TimerManager : MonoBehaviour
    {
        public int LevelStartCountdownTime { get; set; } = 10;
        public int CountdownTime { get; set; } = 30;
        public bool IsTimerRunning { get; set; } = false;
        public bool IsLevelStarted { get; set; } = false;
        public static int MaxCountdownTime { get; set; } = 35;
        public static int ScoreMultiplicationBarrierTime { get; set; } = 30;

        public static TimerManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);    
            }
        }

        private void Update()
        {
            if (!IsLevelStarted && Input.GetKeyDown(KeyCode.X))
            {
                IsLevelStarted = true;
            }
        }
        public void StartCountdown(int durationInSeconds)
        {
            if(!GridManager.Instance.TutorialMode)
                GridManager.Instance.OnLevelLoaded += StartPauseTimer;
            IsTimerRunning = true;  
            CountdownTime = durationInSeconds;
            StartCoroutine(Countdown());
        }

        public void StartPauseTimer()
        {
            if (CountdownTime < 0)
                CountdownTime = 0;
            if(GridManager.Instance.LevelCount != 1)
            {
                StartCoroutine(TimerManager.Instance.LevelStartCountdown());
            }
        }
        public IEnumerator LevelStartCountdown()
        {
            IsTimerRunning = false;
            while (LevelStartCountdownTime > 0)
            {
                if (IsLevelStarted)
                {
                    IsTimerRunning = true;
                    LevelStartCountdownTime = 10;
                    yield break;
                }

                var popupPosition = GridManager.Instance.GridToWorldPosition((GridManager.Instance.Width - 1) / 2, (GridManager.Instance.Height - 1) / 2);

                UIManager.Instance.ShowPopup(popupPosition, Color.green, LevelStartCountdownTime.ToString());
                yield return new WaitForSeconds(1);
                LevelStartCountdownTime--;
            }
            IsTimerRunning = true;
            IsLevelStarted = true;
            LevelStartCountdownTime = 10;
        }
        private IEnumerator Countdown()
        {
            while (CountdownTime > -2)
            {
                yield return new WaitUntil(() => !GridManager.Instance.GameStopped);
                yield return new WaitUntil(()=>IsTimerRunning);
                UIManager.Instance.UpdateTimerDisplay(CountdownTime);
                yield return new WaitForSeconds(1);
                CountdownTime--;
            }

            UIManager.Instance.UpdateTimerDisplay(0);
            if(!GridManager.Instance.TutorialMode)
                GridManager.Instance.FinishGame();
            else
            {
                StartCountdown(30);
            }
        }
        public void IncreaseTimer(int amount, Vector3 popupPlace)
        {
            if (CountdownTime < 0)
                CountdownTime = 0;
            if(CountdownTime + amount > MaxCountdownTime)
                CountdownTime = MaxCountdownTime;
            else
            {
                CountdownTime += amount;
            }
            SoundManager.Instance.TimerIncrease();
            UIManager.Instance.ShowPopup(popupPlace, Color.green, amount + "s");
            UIManager.Instance.UpdateTimerDisplay(CountdownTime);
        }

        public void SetCurrentTime(int timeInSeconds)
        {
            if (timeInSeconds > MaxCountdownTime)
                timeInSeconds = MaxCountdownTime;
            CountdownTime = timeInSeconds;
            UIManager.Instance.UpdateTimerDisplay(CountdownTime);
        }
    }
}