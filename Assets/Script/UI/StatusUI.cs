using System;
using Script.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class StatusUI : MonoBehaviour
    {
        [SerializeField] private GameObject statusUI;
        [SerializeField] private TextMeshProUGUI maxHpText;
        [SerializeField] private TextMeshProUGUI atkText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI dashCdText;
        [SerializeField] private TextMeshProUGUI critRateText;
        [SerializeField] private TextMeshProUGUI goldStatusText;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private Button quitStatusButton;
        private bool statusShow;
        private PlayerDate playerDate;

        private void Start()
        {
            SetupComponent();
            SetController();
            SetButtonAddListener();
        }

        private void Update()
        {
            SetGoldText();
        }

        private void SetupComponent()
        {
            playerDate = GetComponent<PlayerDate>();
        }
        
        private void SetController()
        {
            PlayerController.playerInput.Enable();
            PlayerController.playerInput.PlayerAction.Status.performed += context => CheckIsStatusShowOpenStatus();
        }

        private void SetButtonAddListener()
        {
            quitStatusButton.onClick.AddListener(CloseStatus);
        }
        
        private void SetGoldText()
        {
            goldText.text = $"Gold : {playerDate.playerCharacter.Gold}";
        }

        private void CheckIsStatusShowOpenStatus()
        {
            if (statusShow == false)
            {
                OpenStatus();
            }
            else
            {
                CloseStatus();
            }
            
        }

        private void OpenStatus()
        {
            SetStatusText();
            EnableStatusUI();
            SetBoolStatusShow(true);
            SetTimeScale(0);
            PlayerController.playerInput.Disable();
        }
        
        private void CloseStatus()
        {
            statusUI.SetActive(false);
            DisableStatusUI();
            SetTimeScale(1);
            PlayerController.playerInput.Enable();
        }

        private void EnableStatusUI()
        {
            statusUI.SetActive(true);
        }

        private void DisableStatusUI()
        {
            statusUI.SetActive(false);
        }

        private void SetBoolStatusShow(bool status)
        {
            statusShow = status;
        }

        private void SetTimeScale(int scale)
        {
            Time.timeScale = scale;
        }
        
        private void SetStatusText()
        {
            maxHpText.text = $"{playerDate.playerCharacter.MaxHp}";
            speedText.text = $"{playerDate.playerCharacter.Speed}";
            atkText.text = $"{playerDate.playerCharacter.Atk}";
            dashCdText.text = $"{playerDate.playerCharacter.DashCd}";
            critRateText.text = $"{playerDate.playerCharacter.CritRate}";
            goldStatusText.text = $"{playerDate.playerCharacter.Gold}";
        }
    }
}