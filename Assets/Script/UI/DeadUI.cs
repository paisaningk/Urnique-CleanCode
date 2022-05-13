using System;
using System.Collections;
using Script.Base;
using Script.Controller;
using Script.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class DeadUI : MonoBehaviour
    {
        [SerializeField] private GameObject pauseUi;
        [SerializeField] private GameObject deadUI;
        [SerializeField] private GameObject waveUI;
        private PlayerDate playerDate;

        private void Start()
        {
            SetupComponent();
        }

        private void Update()
        {
            CheckPlayerIsDead();
        }

        private void SetupComponent()
        {
            playerDate = GetComponent<PlayerDate>();
        }

        private void CheckPlayerIsDead()
        {
            if (GetPlayerIsDead())
            {
                StartCoroutine(SetDead());
            }
        }

        private bool GetPlayerIsDead()
        {
            return playerDate.playerCharacter.isDead;
        }

        private IEnumerator SetDead()
        {
            yield return new WaitForSeconds(1);
            SetAllUiDisable();
            PlayerController.playerInput.PlayerAction.Disable();
            Time.timeScale = 0;
        }

        private void SetAllUiDisable()
        {
            pauseUi.SetActive(false);
            waveUI.SetActive(false);
            deadUI.SetActive(true);
        }


    }
}
