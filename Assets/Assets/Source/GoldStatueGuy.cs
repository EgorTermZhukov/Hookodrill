using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GoldStatueGuy : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shake()
    {
        _rectTransform.DOShakeRotation(0.3f, 10f).SetEase(Ease.OutBounce);
    }
}
