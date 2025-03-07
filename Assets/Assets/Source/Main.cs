using Assets.Assets.Source;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] GridManager _gridManagerPrefab;
    [SerializeField] private Transform _endGamePosition;
    private GridManager _gridManager;
    void Start()
    {
        DOTween.Init();
        _gridManager = Instantiate(_gridManagerPrefab);
        _gridManager.CreateLevel(5, 5);
        GridManager.Instance.OnGameEnded += GoToEndgameScreen;
        GridManager.Instance.StartCountdown(60);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadGame();
        }
    }

    private void GoToEndgameScreen(int finalGold)
    {
        Camera.main.transform.position = new Vector3(_endGamePosition.position.x, _endGamePosition.position.y, Camera.main.transform.position.z);
        UIManager.Instance.SetEndgameText(finalGold);
    }

    private static void ReloadGame()
    {
        GameDataManager.Instance.ReloadGame();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
