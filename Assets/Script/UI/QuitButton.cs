using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI
{
    public class QuitButton : MonoBehaviour
    {
        [SerializeField] private Button[] quitButtons;

        private void Start()
        {
            foreach (var button in quitButtons)
            {
                button.onClick.AddListener(Quit);
            }
        }
        
        private void Quit()
        {
            SceneManager.LoadScene("Scenes/Takuma");
        }
    }
}