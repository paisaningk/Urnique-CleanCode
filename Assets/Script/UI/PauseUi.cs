using System;
using Script.Controller;
using Script.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class PauseUi : MonoBehaviour
    {
        [SerializeField] private GameObject pauseUi;
        [SerializeField] private Button resumeButton;
        [SerializeField] private ShopController shopController;

        private void Start()
        {
            SetController();
            SetButtonAddListener();
        }

        private void SetController()
        {
            PlayerController.playerInput.Enable();
            PlayerController.playerInput.PlayerAction.Pause.performed += context => IsPausing();
        }

        private void SetButtonAddListener()
        {
            resumeButton.onClick.AddListener(Resume);
        }

        private void IsPausing()
        {
            if (GetCheckPauseUiIsEnable())
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        private bool GetCheckPauseUiIsEnable()
        {
            return pauseUi.activeInHierarchy;
        }

        private void Pause()
        {
            PlayerController.playerInput.PlayerAction.Disable();
            pauseUi.SetActive(true);
            SetTimeScale(0);
        }

        private void Resume()
        {
            pauseUi.SetActive(false);
            SetTimeScale(1);
            PlayerController.playerInput.PlayerAction.Enable();
        }
        
        private void SetTimeScale(int scale)
        {
            Time.timeScale = scale;
        }
    }
}