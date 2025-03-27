using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

namespace Assets.Assets.Source
{
    internal class GameDataManager : MonoBehaviour
    {
        private int _amountOfGoldInInventory = 0;
        public int CurrentBest { get; private set; }  = 0;
        public int HookPowerCount { get; set; } = 0;
        
        public static int WinCondition = 500;
        private bool _analyticsInitialized = false;

        public int LevelCount { get; set; }
        public int AmountOfGoldInInventory
        {
            get { return _amountOfGoldInInventory; }
            set
            {
                _amountOfGoldInInventory = value;
                UIManager.Instance.SetGoldText(value);
            }
        }
        private async void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            _analyticsInitialized = true;
        }

        public void FinishGame()
        {
            if (_amountOfGoldInInventory > CurrentBest)
            {
                CurrentBest = _amountOfGoldInInventory;
            }
            UIManager.Instance.SetHighscoreText(CurrentBest);
            if (!_analyticsInitialized)
                return;
            var currentPlaytime = Time.time;
            CustomEvent analyticsEvent = new CustomEvent("GameLost")
            {
                { "level_count", LevelCount },
                { "amount_of_gold", AmountOfGoldInInventory },
                { "hook_power_count", HookPowerCount},
                { "current_game_playtime", TimeSpan.FromSeconds(currentPlaytime).Minutes + " m " + TimeSpan.FromSeconds(currentPlaytime).Seconds + " s" }
            };
            LevelCount = 0;
            HookPowerCount = 0;
            AnalyticsService.Instance.RecordEvent(analyticsEvent);
            AnalyticsService.Instance.Flush();
        }
        public void TutorialComplete()
        {
            if (!_analyticsInitialized)
                return;

            float tutorialPlaytime = Time.timeSinceLevelLoad;
            CustomEvent analyticsEvent = new CustomEvent("TutorialComplete")
            {
                {"tutorial_complete", true},
                {"hook_power_count", HookPowerCount},
                {"tutorial_playtime", TimeSpan.FromSeconds(tutorialPlaytime).Minutes + " m " + TimeSpan.FromSeconds(tutorialPlaytime).Seconds + " s"}
            };
            AnalyticsService.Instance.RecordEvent(analyticsEvent);
            AnalyticsService.Instance.Flush();
        }

        public void GameComplete()
        {
            if (!_analyticsInitialized)
                return;
            var currentPlaytime = Time.time;
            CustomEvent analyticsEvent = new CustomEvent("GameComplete")
            {
                {"playtime_till_finished", TimeSpan.FromSeconds(currentPlaytime).Minutes + " m " + TimeSpan.FromSeconds(currentPlaytime).Seconds + " s" }
            };
            AnalyticsService.Instance.RecordEvent(analyticsEvent);
            AnalyticsService.Instance.Flush();
            AnalyticsService.Instance.StopDataCollection();
        }
        public void ReloadGame()
        {
            _amountOfGoldInInventory = 0;
            if (!_analyticsInitialized)
                return;
            CustomEvent analyticsEvent = new CustomEvent("GameRestarted")
            {
            };
            AnalyticsService.Instance.RecordEvent(analyticsEvent);
            AnalyticsService.Instance.Flush();
        }
        public static GameDataManager Instance { get; private set; }
    }
}
