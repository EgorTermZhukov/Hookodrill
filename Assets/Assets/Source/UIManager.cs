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
        [SerializeField] private TextMeshProUGUI _levelText; // Add this field for the timer display
        [SerializeField] private TextMeshPro _endgameText;
        [SerializeField] private TextMeshPro _tutorialText;

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
            _levelText.text = level + "/" + 5;
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
    }
}
