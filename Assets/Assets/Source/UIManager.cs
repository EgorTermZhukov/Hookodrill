using System;
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

        private void Awake()
        {
            Instance = this;
        }
        public void SetGoldText(int amountOfGold)
        {
            _goldAmountText.text = "Gold: " + amountOfGold;
        }
    }
}
