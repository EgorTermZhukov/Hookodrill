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
        public int HookPowerupStash { get; private set; } = 0;
        public int AmountOfGoldInInventory
        {
            get { return _amountOfGoldInInventory; }
            set
            {
                _amountOfGoldInInventory = value;
                HookPowerupStash++;
                UIManager.Instance.SetGoldText(value);
            }
        }
        public bool UsePowerup()
        {
            return true;
            //if (HookPowerupStash < 5)
            //    return false;
            //HookPowerupStash -= 5;
            //return true;
        }
        private void Awake()
        {
            Instance = this;
        }
        public void ReloadGame()
        {
            Destroy(Instance);
        }

        public static GameDataManager Instance { get; private set; }
    }
}
