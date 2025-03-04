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
            Instance = this;
        }

        public static GameDataManager Instance { get; private set; }
    }
}
