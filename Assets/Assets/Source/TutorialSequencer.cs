using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Source;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace Assets.Assets.Source
{
    public class TutorialSequencer : MonoBehaviour
    {
        [SerializeField] private GridManager gridManager;
        [SerializeField] private GameObject textboxViniette;
        [SerializeField] private GameObject timerViniette;
        [SerializeField] private GameObject arrow;
        [SerializeField] private GameObject moveInputs;
        [SerializeField] private GameObject hookInputs;
        [SerializeField] private GameObject skipText;

        private bool _tutorialSkipped = false;
        public void PlayTutorial()
        {
            StartCoroutine(TutorialSequence());
        }
        private IEnumerator TutorialSequence()
        {
            Instantiate(gridManager);
            GridManager.Instance.TutorialMode = true;
            GridManager.Instance.CreateLevel(5, 5);
            GridManager.Instance.GameStopped = true;
            TimerManager.Instance.IsTimerRunning = false;
            
            textboxViniette.SetActive(true);
            
            TextBoxManager.Instance.WriteText("So, you're finally awake", guyShake:false);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);

            TextBoxManager.Instance.WriteText("Welcome to your new gold-hunting grounds - try not to die down here.");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            skipText.SetActive(false);
            TextBoxManager.Instance.WriteText("Quit gawking and MOVE!", textShake:true);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("What's wrong? Never operated legs before?");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);

            TextBoxManager.Instance.WriteText("Fine, basic walking lesson:");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);

            TextBoxManager.Instance.WriteText("Hold [Z] to move, press [ARROW KEYS] to change direction");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Now, Collect that gold for me", showC:false);
            
            textboxViniette.SetActive(false);
            moveInputs.SetActive(true);
            GridManager.Instance.GameStopped = false;
            
            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 1);
            TextBoxManager.Instance.WriteText("Alright a little more", showC:false);
            
            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 2);
            TextBoxManager.Instance.WriteText("More!", GoldGuyFace.TongueOut, false, textShake:true);
            
            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 3);
            
            moveInputs.SetActive(false);
            GridManager.Instance.GameStopped = true;
            
            textboxViniette.SetActive(true);
            
            TextBoxManager.Instance.WriteText("Doing good, i see");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Time for the main event - try not to wet yourself.");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Press [X] to throw your hook o' drill", showC:false, textShake:true);
            
            hookInputs.SetActive(true);
            
            textboxViniette.SetActive(false);
            GridManager.Instance.GameStopped = false;

            GridManager.Instance.ObstaclesSpawning = true;

            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 4);
            //Timer thing
            
            hookInputs.SetActive(false);
            textboxViniette.SetActive(true);
            
            GridManager.Instance.GameStopped = true;
            
            TextBoxManager.Instance.WriteText("See those dark blocks? Impenetrable - for now.");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);

            TextBoxManager.Instance.WriteText("Bump into them to reset their state.");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            GridManager.Instance.GameStopped = false;
            textboxViniette.SetActive(false);
            yield return new WaitUntil(()=> GridManager.Instance.LevelCount > 5);
            
            GridManager.Instance.GameStopped = true;
            timerViniette.SetActive(true);
            
            TextBoxManager.Instance.WriteText("This timer is your lifeforce");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            arrow.SetActive(false);
            
            TextBoxManager.Instance.WriteText("I really like the number 3", GoldGuyFace.TongueOut);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            GridManager.Instance.ShowShine();
            TextBoxManager.Instance.WriteText("Chain 3 gold in one throw and i will give you 3 seconds!");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("There can be gaps between them", showC:false);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("This is your ONLY way to survive", GoldGuyFace.TongueOut, textShake:true, showC: false);
            

            TimerManager.Instance.StartCountdown(30);
            
            timerViniette.SetActive(false);
            GridManager.Instance.GameStopped = false;
            
            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 6);

            TimerManager.Instance.IsTimerRunning = false;
            GridManager.Instance.GameStopped = true;
            
            textboxViniette.SetActive(true);
            
            TextBoxManager.Instance.WriteText("This is too good to be true");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("The timer cannot reach above 35 seconds", GoldGuyFace.TongueOut);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("You get score boost if you hold it above 30");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("...");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("I know this might be too difficult");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("That's why timer is frozen for 10 seconds at start of each level");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("But if you move the time will run again");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Try it out, and then get me some gold again!", showC:false);
            
            textboxViniette.SetActive(false);
            GridManager.Instance.GameStopped = false;
            
            TimerManager.Instance.StartPauseTimer();

            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 7);
            
            textboxViniette.SetActive(true);
            GridManager.Instance.GameStopped = true;
            
            TextBoxManager.Instance.WriteText("Right right, one last thing");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("The cave grows every 5 levels");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText($"If you get {GameDataManager.WinCondition}$ you may get on with your life", textShake:true);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Good luck", GoldGuyFace.TongueOut);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            DOTween.KillAll();
            GameDataManager.Instance.TutorialComplete();
            GameDataManager.Instance.ReloadGame();
            //SceneManager.LoadScene(SceneManager.GetSceneByName("MainScene").buildIndex);
            SceneManager.LoadScene("Scenes/MainScene");
        }
        public void Update()
        {
            if (_tutorialSkipped)
                return;
            if (Input.GetKeyDown(KeyCode.U))
            {
                GameDataManager.InfiniteMode = true;
                DOTween.KillAll();
                _tutorialSkipped = true;
                StopAllCoroutines();
                GameDataManager.Instance.SkipTutorial();
                GameDataManager.Instance.ReloadGame();
                SceneManager.LoadScene("Scenes/MainScene");
            }
        }
    }
}