using Assets.Assets.Source;
using Assets.Source;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Main : MonoBehaviour
{
    [SerializeField] GridManager _gridManagerPrefab;
    [SerializeField] private Transform _endGamePosition;
    [SerializeField] private Transform _gameWinPosition;
    private GridManager _gridManager;
    private bool gameWasComplete = false;

    private bool _gameStarted = false;
    void Start()
    {
        DOTween.Init();
        _gridManager = Instantiate(_gridManagerPrefab);
        _gridManager.CreateLevel(5, 5);
        GridManager.Instance.OnGameEnded += GoToEndgameScreen;
    }
    void Update()
    {
        if (gameWasComplete)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameDataManager.InfiniteMode = true;
                ReloadGame();
            }
            return;
        }
        if (GameDataManager.Instance.AmountOfGoldInInventory >= GameDataManager.WinCondition && !GameDataManager.InfiniteMode)
        {
            gameWasComplete = true;
            GridManager.Instance.WinGame();
            GoToWinScreen();
            return;
        }
        if (!_gameStarted && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            _gameStarted = true;
            TimerManager.Instance.StartCountdown(30);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GridManager.Instance.GameStopped)
                GridManager.Instance.GameStopped = false;
            else
            {
                GridManager.Instance.GameStopped = true;
            }
        }
    }

    private void GoToEndgameScreen(int finalGold)
    {
        GameDataManager.Instance.FinishGame();
        DOTween.KillAll();
        Camera.main.transform.position = new Vector3(_endGamePosition.position.x, _endGamePosition.position.y, Camera.main.transform.position.z);
        UIManager.Instance.SetEndgameText(finalGold);
    }

    private void GoToWinScreen()
    {
        GameDataManager.Instance.GameComplete();
        GameDataManager.Instance.CurrentBest = GameDataManager.Instance.AmountOfGoldInInventory;
        DOTween.KillAll();
        Camera.main.transform.position = new Vector3(_gameWinPosition.position.x, _gameWinPosition.position.y, Camera.main.transform.position.z);
    }
    public static void ReloadGame()
    {
        DOTween.KillAll();
        GameDataManager.Instance.ReloadGame();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
