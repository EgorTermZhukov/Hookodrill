using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Assets.Source
{
    public class TutorialSequencer : MonoBehaviour
    {
        private bool _isAllowedToGoNext = false;

        public void PlayTutorial()
        {
            StartCoroutine(TutorialSequence());
        }
        private IEnumerator TutorialSequence()
        {
            TextBoxManager.Instance.WriteText("Who stumbled into my mine? Go away!");
            yield return new WaitUntil(IsAllowedToGoNext);
            _isAllowedToGoNext = false;
            TextBoxManager.Instance.WriteText("Cmon, it's like you don't know how to do it, pathetic");
            yield return new WaitUntil(IsAllowedToGoNext);
            _isAllowedToGoNext = false;
            TextBoxManager.Instance.WriteText("If i were you i would try using your arrow keys or something like that to look around");
            yield return new WaitUntil(IsAllowedToGoNext);
            _isAllowedToGoNext = false;
            TextBoxManager.Instance.WriteText("Damn, you should try pressing Z now, i've heard it moves you by 1 cell?");
            yield return new WaitUntil(IsAllowedToGoNext);
            _isAllowedToGoNext = false;
            TextBoxManager.Instance.WriteText("Alright alright, last thing, you can throw your hook by pressing X");
            yield return new WaitUntil(IsAllowedToGoNext);
            SceneManager.LoadScene(SceneManager.GetSceneByName("MainScene").buildIndex + 1);
        }
        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
                _isAllowedToGoNext = true;
        }
        public bool IsAllowedToGoNext()
        {
            return _isAllowedToGoNext;
        }
    }
}