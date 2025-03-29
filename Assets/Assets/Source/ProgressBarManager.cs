using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressBarManager : MonoBehaviour
{
    [SerializeField] private GameObject progressBarParent;
    [SerializeField] private Image progressbarBackground;
    [SerializeField] private Image progressBar;
    
    public static ProgressBarManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        UpdateProgressBar(0f);
    }
    void Update()
    {
        
    }
    public void UpdateProgressBar(float progress)
    {
        progressBar.fillAmount = progress;
    }

    public void Reset()
    {
        UpdateProgressBar(0f);
    }

    public void Hide()
    {
        progressBarParent.SetActive(false);
    }
    public void Show()
    {
        progressBarParent.SetActive(true);
    }
}
