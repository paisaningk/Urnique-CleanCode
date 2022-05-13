using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI
{
    public class RestartButton : MonoBehaviour
    {
        [SerializeField] private Button[] restartButtons;
        
        private void Start()
        {
            foreach (var button in restartButtons)
            {
                button.onClick.AddListener(Restart);
            }
        }
        
        private void Restart()
        {
            SceneManager.LoadScene("Scenes/Map1");
        }
    }
}