using Assets.Assets.Source;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Source
{
    public class TutorialMain : MonoBehaviour
    {
        [SerializeField] TutorialSequencer _tutorialSequencerPrefab;
        private TutorialSequencer _tutorialSequencerInstance;
        void Start()
        {
            _tutorialSequencerInstance = Instantiate(_tutorialSequencerPrefab);
            _tutorialSequencerInstance.PlayTutorial();
        }
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
