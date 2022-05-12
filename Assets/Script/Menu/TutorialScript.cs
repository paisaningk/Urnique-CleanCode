using UnityEngine;
using UnityEngine.UI;

namespace Script.Menu
{
    public class TutorialScript : MonoBehaviour
    {
        [SerializeField] private GameObject tutorial;
        [SerializeField] private Button close;
        void Start()
        {
            close.onClick.AddListener(CloseUi);
        }

        private void CloseUi()
        {
            tutorial.SetActive(false);
        }
    }
}
