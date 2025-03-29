using UnityEngine;

public class GameDataManagerNew : MonoBehaviour
{
    public static GameDataManagerNew Instance;
    
    public int CurrentGoal = 100;
    public int AmountOfGoldInInventory = 0;
    
    public bool InfiniteMode = false;

    [SerializeField] public int GoalIncrease = 100;
    [SerializeField] public int GoldRequirment = 200;
    [SerializeField] public int IncreaseAmount = 10;
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        ResetInventory();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ResetInventory()
    {
        AmountOfGoldInInventory = 0;
        ProgressBarManager.Instance.UpdateProgressBar(0);
    }
    public void IncreaseAmountOfGold(int increaseAmount)
    {
        AmountOfGoldInInventory += increaseAmount;
        float progress;
        if (AmountOfGoldInInventory > GoldRequirment)
        {
            AmountOfGoldInInventory = GoldRequirment;
        }
        progress = AmountOfGoldInInventory * (1f / GoldRequirment);
        //else
        //{
        //    if (AmountOfGoldInInventory > CurrentGoal)
        //    {
        //        CurrentGoal += GoalIncrease;
        //        Debug.Log("New goal: " + CurrentGoal);
        //    }
        //    progress = (AmountOfGoldInInventory % CurrentGoal) * (1.0f / CurrentGoal);
        //}
        
        ProgressBarManager.Instance.UpdateProgressBar(progress);
    }
}
