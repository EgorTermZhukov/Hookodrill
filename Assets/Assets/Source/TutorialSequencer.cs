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
            
            TextBoxManager.Instance.WriteText("So you're awake, haha", guyShake:false);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Such carelessness!", GoldGuyFace.TongueOut);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("What were you thinking drilling gold in an old mine like that?");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Well you're stuck there now");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("What's wrong? Do what you came here for already!");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Drill that damn gold!", textShake:true, showC:false);
            
            textboxViniette.SetActive(false);
            moveInputs.SetActive(true);
            GridManager.Instance.GameStopped = false;
            
            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 1);
            TextBoxManager.Instance.WriteText("Oh your pockets are full of holes", showC:false);
            
            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 2);
            TextBoxManager.Instance.WriteText("<shake MaxYAmplitude=3 MaxXAmplitude=3> You're so funny! </shake>", GoldGuyFace.TongueOut, false, textShake:true);
            
            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 3);
            
            moveInputs.SetActive(false);
            GridManager.Instance.GameStopped = true;
            
            textboxViniette.SetActive(true);
            
            TextBoxManager.Instance.WriteText("Wait, hold on a second", GoldGuyFace.TongueOut);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Your <wave>Drill</wave> can do much more than it seems");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("<wave>Press X</wave> to throw it", showC:false, textShake:true);
            
            hookInputs.SetActive(true);
            
            textboxViniette.SetActive(false);
            GridManager.Instance.GameStopped = false;

            GridManager.Instance.ObstaclesSpawning = true;

            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 4);
            //Timer thing
            
            hookInputs.SetActive(false);
            textboxViniette.SetActive(true);
            
            GridManager.Instance.GameStopped = true;
            
            TextBoxManager.Instance.WriteText("How could you not know what your own tool does");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("You see that it goes through the blocks?");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("There is a type of block your hook can't break through");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Those are <wave>slightly darker</wave>");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);

            TextBoxManager.Instance.WriteText("Move into them to reset their state");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            GridManager.Instance.GameStopped = false;
            textboxViniette.SetActive(false);
            yield return new WaitUntil(()=> GridManager.Instance.LevelCount > 5);
            
            GridManager.Instance.GameStopped = true;
            
            textboxViniette.SetActive(true);
            TextBoxManager.Instance.WriteText("You realize that you gotta play by my rules now?");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            textboxViniette.SetActive(false);
            
            timerViniette.SetActive(true);
            
            TextBoxManager.Instance.WriteText("Now now, this is your timer");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            arrow.SetActive(false);
            
            TextBoxManager.Instance.WriteText("If it runs out, you die", textShake:true);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("<shake>I love the number 3</shake>", GoldGuyFace.TongueOut);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            GridManager.Instance.ShowShine();
            TextBoxManager.Instance.WriteText("Collect 3 gold in one hook throw and i will give you 3 seconds");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Doing that consistently is your <wave>only survival strategy</wave>", GoldGuyFace.TongueOut, textShake:true);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("There can be <wave>gaps</wave> in between the blocks");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TimerManager.Instance.StartCountdown(30);
            
            timerViniette.SetActive(false);
            GridManager.Instance.GameStopped = false;
            
            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 6);

            TimerManager.Instance.IsTimerRunning = false;
            GridManager.Instance.GameStopped = true;
            
            textboxViniette.SetActive(true);
            
            TextBoxManager.Instance.WriteText("<wave><palette>Feels so good, right?</palette></wave>", face: GoldGuyFace.TongueOut, textShake:true);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("However");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("The capacity of the timer is 35 seconds", GoldGuyFace.TongueOut);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Your goal is to hold it above 30");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Do this and the gold is <palette color='purple', 'yellow'>worth 2x more</palette>");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("...");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("This might be too difficult to perform at first");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("That's why the time is frozen for 10 seconds at the start of each level");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("If you move, the time will start again");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("You can <wave>turn around</wave> though, i allow it", GoldGuyFace.TongueOut);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Try it out, and get me gold again!", showC:false);
            
            textboxViniette.SetActive(false);
            GridManager.Instance.GameStopped = false;
            
            TimerManager.Instance.StartPauseTimer();

            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 7);
            
            textboxViniette.SetActive(true);
            GridManager.Instance.GameStopped = true;
            
            TextBoxManager.Instance.WriteText("One last thing");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("The map grows every 5 levels");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText($"If you get <wave><palette>{GameDataManager.WinCondition}$</palette></wave> then i might show you something", textShake:true);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("<wave><palette>Goo</palette></wave> luck", GoldGuyFace.TongueOut);
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