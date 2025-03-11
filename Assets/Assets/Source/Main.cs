using Assets.Assets.Source;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Main : MonoBehaviour
{
    [SerializeField] GridManager _gridManagerPrefab;
    [SerializeField] private Transform _endGamePosition;
    private GridManager _gridManager;

    private bool _gameStarted = false;
    void Start()
    {
        DOTween.Init();
        _gridManager = Instantiate(_gridManagerPrefab);
        _gridManager.CreateLevel(5, 5);
        GridManager.Instance.OnGameEnded += GoToEndgameScreen;
        UIManager.Instance.DisplayTutorial();
    }
    void Update()
    {
        if (!_gameStarted && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            UIManager.Instance.HideTutorial();
            GridManager.Instance.StartCountdown(60);
            _gameStarted = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadGame();
        }
    }

    private void GoToEndgameScreen(int finalGold)
    {
        DOTween.KillAll();
        Camera.main.transform.position = new Vector3(_endGamePosition.position.x, _endGamePosition.position.y, Camera.main.transform.position.z);
        UIManager.Instance.SetEndgameText(finalGold);
    }

    private static void ReloadGame()
    {
        DOTween.KillAll();
        GameDataManager.Instance.ReloadGame();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
