using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Assets.Source
{
    internal class GameDataManager : MonoBehaviour
    {
        private int _amountOfGoldInInventory = 0;
        public int CurrentBest { get; private set; }  = 0;
        public static int WinCondition = 666;

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
        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
            {
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this.gameObject);
        }

        public void FinishGame()
        {
            if (_amountOfGoldInInventory > CurrentBest)
            {
                CurrentBest = _amountOfGoldInInventory;
            }
            UIManager.Instance.SetHighscoreText(CurrentBest);
        }
        public void ReloadGame()
        {
            _amountOfGoldInInventory = 0;
        }

        public static GameDataManager Instance { get; private set; }
    }
}
