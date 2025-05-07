using Assets.Assets.Source;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GoldStatueGuy : MonoBehaviour
{
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _tongueOut;
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
        _rectTransform.DOShakeAnchorPos(0.5f, 2f);
    }

    public void SetFace(GoldGuyFace face)
    {
        Sprite currentFace = _defaultSprite;
        switch (face)
        {
            case GoldGuyFace.TongueOut:
                currentFace = _tongueOut;
                break;
        }
        GetComponent<Image>().sprite = currentFace;
    }
}
