using System;
using TMPro;
using UnityEngine;

namespace Assets.Assets.Source
{
    public class TextBoxManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMesh;
        [SerializeField] private GoldStatueGuy _goldStatueGuy;
        public static TextBoxManager Instance { get; private set; }
        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        public void WriteText(string text)
        {
            _textMesh.text = text;
            _goldStatueGuy.Shake();
        }
    }
}