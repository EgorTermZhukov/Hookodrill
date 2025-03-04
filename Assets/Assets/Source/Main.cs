using Assets.Assets.Source;
using UnityEngine;
using DG.Tweening;

public class Main : MonoBehaviour
{
    [SerializeField] GridManager _gridManagerPrefab;
    private GridManager _gridManager;
    void Start()
    {
        DOTween.Init();
        _gridManager = Instantiate(_gridManagerPrefab);
        _gridManager.CreateLevel(10, 10);
        
    }
    void Update()
    {
        
    }
}
