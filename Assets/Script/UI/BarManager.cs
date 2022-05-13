using System;
using Script.Base;
using Script.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class BarManager : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private Image dashBar;
        private PlayerDate playerDate;
        private float dashCoolDown = 0;

        private void Start()
        {
            SetupComponent();
            SetFillFullDasBar();
        }

        private void Update()
        {
            FillHealthBar();
            CheckIsDash();
        }

        private void SetupComponent()
        {
            playerDate = GetComponent<PlayerDate>();
        }

        private void FillHealthBar()
        {
            healthBar.fillAmount = GetHealth();
        }

        private float GetHealth()
        {
            return playerDate.playerCharacter.Hp / playerDate.playerCharacter.MaxHp;
        }

        private void CheckIsDash()
        {
            if (!PlayerMovement.canDash)
            {
                FillDashBar();
            }
            else
            {
                ReSetFillDashBar();
            }
        }
        
        private void FillDashBar()
        {
            dashBar.fillAmount = GetCoolDownDash();
        }

        private void ReSetFillDashBar()
        {
            dashBar.fillAmount = 0;
        }

        private void SetFillFullDasBar()
        {
            dashBar.fillAmount = 1;
        }

        private float GetCoolDownDash()
        {
            dashCoolDown += Time.fixedDeltaTime;
            return dashCoolDown / playerDate.playerCharacter.DashCd;
        }
    }
}