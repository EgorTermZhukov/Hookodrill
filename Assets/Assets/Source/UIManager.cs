using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Assets.Source
{
    internal class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private TextMeshProUGUI _goldAmountText;
        [SerializeField] private TextMeshProUGUI _timerText; // Add this field for the timer display
        [SerializeField] private TextMeshPro _endgameText;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(this);
            }
        }
        public void SetGoldText(int amountOfGold)
        {
            _goldAmountText.text = "Gold: " + amountOfGold;
        }
        // Method to start the countdown

        // Method to update the timer display
        public void UpdateTimerDisplay(int timeInSeconds)
        {
            _timerText.text = "Time: " + timeInSeconds.ToString();
        }
        public void SetEndgameText(int gold)
        {
            Destroy(_goldAmountText);
            Destroy(_timerText);
            _endgameText.text = "Collected gold: " + gold;
        }
    }
}
