using System;
using System.Collections;
using System.Collections.Generic;
using Script.Controller;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    [Header("Text")]
    public TextAsset InkJson;
    public Sprite ImageProfile;
    public string Name;
    public GameObject Button;
    private bool playerInRange = false;
    

    private void Update()
    {
        Button.SetActive(playerInRange);
        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E) && DialogueManager.GetInstance().DialoguePlaying == false)
            {
                DialogueManager.GetInstance().EnterDialogueMode(InkJson,Name,ImageProfile,NpcType.Normal);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("adc");
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (DialogueManager.GetInstance().DialoguePlaying)
            {
                StartCoroutine(DialogueManager.GetInstance().ExitDialogueMode());
            }
        }
    }
}
