using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static Assets.Assets.Source.TextBoxManager;

namespace Assets.Assets.Source
{
    internal class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private TextMeshProUGUI _goldAmountText;
        [SerializeField] private TextMeshProUGUI _timerText; // Add this field for the timer display
        [SerializeField] private TextMeshProUGUI _levelText; // Add this field for the timer display
        [SerializeField] private TextMeshPro _endgameText;
        [SerializeField] private TextMeshPro _tutorialText;
        [SerializeField] private TextMeshPro _highScoreText;
        [SerializeField] private GameObject _dialogue;

        private GameObject _tutorialInstance;

        [Header("Popup Settings")]
        public GameObject popupPrefab;
        public Canvas parentCanvas;
        public float popupDuration = 1f;
        public float popupMoveHeight = 50f;

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
            _goldAmountText.text = "$" + amountOfGold;
        }
        // Method to start the countdown

        // Method to update the timer display
        public void UpdateTimerDisplay(int timeInSeconds)
        {
            _timerText.text = "Time: " + timeInSeconds.ToString();
            if (timeInSeconds < 0)
                _timerText.text = "Time: " + 0;
            if (timeInSeconds > 30)
            {
                _timerText.color = Color.cyan;
            }
            else if  (timeInSeconds > 5)
            {
                _timerText.color = Color.green;
            }
            else
            {
                _timerText.color = Color.red;
            }
        }
        public void SetEndgameText(int gold)
        {
            Destroy(_goldAmountText);
            Destroy(_timerText);
            Destroy(_levelText);
            _endgameText.text = "Collected gold: " + gold;
        }

        public void ShowPopup(Vector3 worldPosition, Color color, string text = "1")
        {
            // Convert world position to screen space
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            // Create the popup
            GameObject popup = Instantiate(popupPrefab, parentCanvas.transform);
            popup.GetComponent<TextMeshProUGUI>().text = text;
            popup.GetComponent<TextMeshProUGUI>().color = color;
            RectTransform rectTransform = popup.GetComponent<RectTransform>();

            // Convert screen position to canvas local position
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.GetComponent<RectTransform>(),
                screenPosition,
                parentCanvas.worldCamera,
                out localPosition);

            rectTransform.localPosition = localPosition;

            // Start animation coroutine
            StartCoroutine(AnimatePopup(popup));
        }
        public void UpdateLevelText(int level)
        {
            _levelText.text = level + "/" + GridManager.Instance.LevelRequirement;
        }

        private IEnumerator AnimatePopup(GameObject popup)
        {
            CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
                canvasGroup = popup.AddComponent<CanvasGroup>();

            RectTransform rectTransform = popup.GetComponent<RectTransform>();
            Vector2 startPosition = rectTransform.localPosition;
            Vector2 endPosition = startPosition + new Vector2(0, popupMoveHeight);

            float elapsed = 0f;

            while (elapsed < popupDuration)
            {
                float t = elapsed / popupDuration;

                // Move upwards
                rectTransform.localPosition = Vector2.Lerp(startPosition, endPosition, t);

                // Fade out
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(popup);
        }

        internal void DisplayTutorial()
        {
            _tutorialText = Instantiate(_tutorialText, Camera.main.transform);
        }
        internal void HideTutorial()
        {
            Destroy(_tutorialText.gameObject);
        }

        public void PlayGameEndSequence()
        {
            StartCoroutine(GameEndSequence());
        }

        public void SetHighscoreText(int amountOfGoldInInventory)
        {
            _highScoreText.text = "Current score: " + amountOfGoldInInventory;
        }

        private IEnumerator GameEndSequence()
        {
            _dialogue.SetActive(true);
            TextBoxManager.Instance.WriteText($"You got {GameDataManager.Instance.AmountOfGoldInInventory}$");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            TextBoxManager.Instance.WriteText($"Your best was {GameDataManager.Instance.CurrentBest}$");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            TextBoxManager.Instance.WriteText($"You need {GameDataManager.WinCondition}$");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            if (GameDataManager.Instance.AmountOfGoldInInventory < 200)
            {
                TextBoxManager.Instance.WriteText("Good, but not enough, try again!");
                yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            }
            else if(GameDataManager.Instance.AmountOfGoldInInventory < 300)
            {
                TextBoxManager.Instance.WriteText("That is pretty close, you're getting good at this!");
                yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
                TextBoxManager.Instance.WriteText("But it's still not enough, try again!");
                yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            }
            else
            {
                TextBoxManager.Instance.WriteText("You almost did it!", GoldGuyFace.TongueOut);
                yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
                TextBoxManager.Instance.WriteText("Just " + (GameDataManager.WinCondition - GameDataManager.Instance.AmountOfGoldInInventory).ToString() + "$ left!", GoldGuyFace.TongueOut);
                yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
                TextBoxManager.Instance.WriteText("Try again, you can definitely do it this time");
                yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            }
            Main.ReloadGame();
        }
    }
}
