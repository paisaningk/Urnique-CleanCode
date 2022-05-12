using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour
{
    public GameObject Button;
    public GameObject Neo;
    private bool playerInRange;
    private void Start()
    {
        Neo.SetActive(false);
        Button.SetActive(false);
        playerInRange = false;
        
    }

    public void Update()
    {
        Button.SetActive(playerInRange);
        if (Input.GetKeyDown(KeyCode.E)&&playerInRange)
        {
            Neo.SetActive(true);
            Button.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Neo.SetActive(false);
        }
    }
}
