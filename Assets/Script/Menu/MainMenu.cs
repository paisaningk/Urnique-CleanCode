using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.Menu
{
    //this Script use in Main Menu Scene
    //It is responsible for change Scene.
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button tutorialButton;
        [SerializeField] private Button quitButton;
        public MMFeedbacks LoadScene;

        private void Awake()
        {
            startButton.onClick.AddListener(StartGame);
            quitButton.onClick.AddListener(QuitGame);
            tutorialButton.onClick.AddListener(Tutorial);
            Time.timeScale = 1;
        }

        private void Tutorial()
        {
            SceneManager.LoadScene("ChooseCharacterTutorial");
        }

        private void StartGame()
        {
            LoadScene?.PlayFeedbacks();
        }

        private void QuitGame()
        {
            Application.Quit();
            Debug.Log("it work");
        }
    }
}
