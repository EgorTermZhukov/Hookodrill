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
            
            TextBoxManager.Instance.WriteText("So, you're finally awake");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("You fell through a hole in the ground, you are in my cave now");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Stand up and move!");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Cmon, it's like you don't know how to do it, pathetic");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Let me tell you");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);

            TextBoxManager.Instance.WriteText("Hold [Z] to move, press [ARROW KEYS] to change direction");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Collect all the gold for me", showC:false);
            
            textboxViniette.SetActive(false);
            moveInputs.SetActive(true);
            GridManager.Instance.GameStopped = false;
            
            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 1);
            TextBoxManager.Instance.WriteText("Alright a little more", showC:false);
            
            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 2);
            TextBoxManager.Instance.WriteText("More!", GoldGuyFace.TongueOut, false);
            
            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 3);
            
            moveInputs.SetActive(false);
            GridManager.Instance.GameStopped = true;
            
            textboxViniette.SetActive(true);
            
            TextBoxManager.Instance.WriteText("Doing good, i see");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Let me show you the coolest thing ever");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Throw your hook with an [X] key");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            hookInputs.SetActive(true);
            
            textboxViniette.SetActive(false);
            GridManager.Instance.GameStopped = false;

            GridManager.Instance.ObstaclesSpawning = true;

            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 4);
            //Timer thing
            
            hookInputs.SetActive(false);
            textboxViniette.SetActive(true);
            
            GridManager.Instance.GameStopped = true;
            
            TextBoxManager.Instance.WriteText("You're seeing how it goes through all the blocks?");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("There is a type of block your hook can't break through");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Those are slightly darker");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);

            TextBoxManager.Instance.WriteText("You can turn them back to normal by moving into them");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            GridManager.Instance.GameStopped = false;
            textboxViniette.SetActive(false);
            yield return new WaitUntil(()=> GridManager.Instance.LevelCount > 5);
            
            GridManager.Instance.GameStopped = true;
            timerViniette.SetActive(true);
            
            TextBoxManager.Instance.WriteText("Now, this is your timer");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            arrow.SetActive(false);
            
            TextBoxManager.Instance.WriteText("If it runs out, you die");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("I really like the number 3", GoldGuyFace.TongueOut);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Hook 3 gold in a line and i will give you 3 seconds");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("This is your ONLY way to survive", GoldGuyFace.TongueOut);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Go try it out!", showC:false);

            TimerManager.Instance.StartCountdown(30);
            
            timerViniette.SetActive(false);
            GridManager.Instance.GameStopped = false;
            
            yield return new WaitUntil(() => GridManager.Instance.LevelCount > 6);

            TimerManager.Instance.IsTimerRunning = false;
            GridManager.Instance.GameStopped = true;
            
            textboxViniette.SetActive(true);
            
            TextBoxManager.Instance.WriteText("Too good to be true, right? Right!");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("The timer can only get up to 35 seconds", GoldGuyFace.TongueOut);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("If it's higher than 30 seconds you get 2x from collected gold");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("You also have 10 seconds at the start of each level");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("This is to let you think for a while");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Because i really want those 3s", GoldGuyFace.TongueOut);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("You can change direction");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("Just make sure you dont move and you're safe for a while");
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
            
            TextBoxManager.Instance.WriteText("The map grows every 5 levels");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText($"If you get {GameDataManager.WinCondition}$ i will let you free");
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            
            TextBoxManager.Instance.WriteText("You can't run from me", GoldGuyFace.TongueOut);
            yield return new WaitUntil(TextBoxManager.Instance.IsDialogueComplete);
            DOTween.KillAll();
            GameDataManager.Instance.ReloadGame();
            //SceneManager.LoadScene(SceneManager.GetSceneByName("MainScene").buildIndex);
            SceneManager.LoadScene("Scenes/MainScene");
        }
        public void Update()
        {
        }
    }
}