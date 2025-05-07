using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Assets.Assets.Source
{
    public enum GoldGuyFace
    {
        Default,
        TongueOut
    }
    public class TextBoxManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMesh;
        [SerializeField] private GoldStatueGuy _goldStatueGuy;
        [SerializeField] private GameObject _pressCText;

        private bool _dialogueComplete = false;
        
        public static TextBoxManager Instance { get; private set; }
        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Space))
            {
                _dialogueComplete = true;
            }
        }

        public void WriteText(string text, GoldGuyFace face = GoldGuyFace.Default, bool showC = true, bool guyShake = true, bool textShake = false)
        {
            if(showC)
                _pressCText.SetActive(true);
            else
                _pressCText.SetActive(false);
            SoundManager.Instance.CharacterTalk();
            _textMesh.text = text;
            _dialogueComplete = false;
            _goldStatueGuy.SetFace(face);
            if(guyShake)
                _goldStatueGuy.Shake();
            if(textShake)
                _textMesh.rectTransform.DOShakeAnchorPos(0.5f, 3f);
        }
        public bool IsDialogueComplete()
        {
            if(_dialogueComplete)
                _pressCText.SetActive(false);
            return _dialogueComplete;
        }
    }
}