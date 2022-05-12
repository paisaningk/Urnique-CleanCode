using System.Collections;
using System.Collections.Generic;
using Script.Spawn;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangMode : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private TextMeshProUGUI Text;
    // Start is called before the first frame update
    void Start()
    {
        Button.onClick.AddListener(Mode);
        Text.text = $"{SpawnPlayer.instance.Mode}";
    }

    private void Mode()
    {
        if (SpawnPlayer.instance.Mode == global::Mode.Easy)
        {
            SpawnPlayer.instance.Mode = global::Mode.Hard;
            Text.text = $"{SpawnPlayer.instance.Mode}";
        }
        else
        {
            SpawnPlayer.instance.Mode = global::Mode.Easy;
            Text.text = $"{SpawnPlayer.instance.Mode}";
        }
    }

}
