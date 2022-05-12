using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpToGameplay : MonoBehaviour
{
    public GameObject[] Icons;
    public MMFeedbacks GamePlay;
    private bool playerInRange = false;
    private bool warp = true;
    private void Update()
    {
        if (playerInRange && warp)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GamePlay.PlayFeedbacks();
                warp = false;
            }
        }   
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            foreach (var icon in Icons)
            {
                icon.SetActive(playerInRange);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            foreach (var icon in Icons)
            {
                icon.SetActive(playerInRange);
            }
        }
    }
}
