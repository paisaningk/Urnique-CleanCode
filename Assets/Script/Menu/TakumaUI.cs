using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Script.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TakumaUI : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private GameObject pauseUi;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button pauseButton;
    private bool isPause = false;

    private void Awake()
    {
        pauseButton.onClick.AddListener(Resume);
        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(Quit);
    }

    public void Resume()
    {
        if (isPause == false)
        {
            pauseButton.gameObject.SetActive(isPause);
            isPause = true;
            pauseUi.SetActive(isPause);
        }
        else
        {
            pauseButton.gameObject.SetActive(isPause);
            isPause = false;
            pauseUi.SetActive(isPause);
        }
    }


    private void Quit()
    {
        SceneManager.LoadScene($"{SceneName.MainMenu_PC}");
    }
}
