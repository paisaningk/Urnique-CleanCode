using System;
using System.Collections;
using MoreMountains.Feedbacks;
using Script.Base;
using Script.Controller;
using Script.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.Menu
{
    public class GameplaySceneMenu : MonoBehaviour
    {
        [Header("Script")]
        [SerializeField] private ShopController shopController;
        [Header("UI")]
        [SerializeField] private GameObject pauseUi;
        [SerializeField] private GameObject deadUI;
        [SerializeField] private GameObject waveUI;
        [SerializeField] private GameObject StatusUI;
        [Header("Button")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button deadquitButton;
        [SerializeField] private Button deadwinButton;
        [SerializeField] private Button restartwinButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button restartpauseButton;
        [Header("Player")]
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private PlayerCharacter PlayerCharacter;
        [SerializeField] private Image blood;
        [SerializeField] private Image Dash;
        [Header("Status")]
        [SerializeField] private TextMeshProUGUI MaxHPText;
        [SerializeField] private TextMeshProUGUI AtkText;
        [SerializeField] private TextMeshProUGUI SpeedText;
        [SerializeField] private TextMeshProUGUI DashCdText;
        [SerializeField] private TextMeshProUGUI CritRateText;
        [SerializeField] private TextMeshProUGUI GoldText;
        [SerializeField] private Button quitStatusButton;
        [Header("Gun")] 
        [SerializeField] private GameObject Ammoui;
        [SerializeField] private GameObject[] Ammo;
        [SerializeField] private GameObject AmmoText;
        [Header("Gun")]
        public MMFeedbacks BackToHub;
        public MMFeedbacks PlayAgain;
        
        private PlayerController playerController;
        public bool isPause = false;
        private float DashCd = 0;
        private bool candash = true;
        private bool StatusShow = false;
        private bool isReload;
        private GameObject player;
        

        private void Awake()
        {
            Time.timeScale = 1;
            resumeButton.onClick.AddListener(Resume);
            quitButton.onClick.AddListener(Quit);
            deadquitButton.onClick.AddListener(Quit);
            deadwinButton.onClick.AddListener(Quit);
            restartButton.onClick.AddListener(Restart);
            restartwinButton.onClick.AddListener(Restart);
            restartpauseButton.onClick.AddListener(Restart);
            quitStatusButton.onClick.AddListener(Back);
            Dash.fillAmount = 1;
        }

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            PlayerCharacter = player.GetComponent<PlayerCharacter>();
            playerController = player.GetComponent<PlayerController>();
            Ammoui.SetActive(PlayerCharacter.PlayerType == PlayerType.Gun);
            PlayerController.playerInput.PlayerAction.Status.performed += context => OpenStatus();
            if (PlayerCharacter.PlayerType == PlayerType.Gun)
            {
                PlayerController.playerInput.PlayerAction.Reload.performed += context => Reloadative();
            }
            PlayerController.playerInput.Enable();
        }

        private void Update()
        {
            if (PlayerCharacter.PlayerType == PlayerType.Gun)
            {
                var ammo = playerController.Ammo;

                if (isReload)
                {
                    foreach (var VARIABLE in Ammo)
                    {
                        VARIABLE.SetActive(true);
                    }
                    isReload = false;
                }
                else if (ammo == 1)
                {
                    Ammo[ammo-1].SetActive(false);
                }
                else if (ammo == 2)
                {
                    Ammo[ammo-1].SetActive(false);
                }
                else if (ammo == 3)
                {
                    Ammo[ammo-1].SetActive(false);
                }
                else if (ammo == 4)
                {
                    Ammo[ammo-1].SetActive(false);
                }
                else if (ammo == 5)
                {
                    Ammo[ammo-1].SetActive(false);
                    StartCoroutine(Reload());
                }
            }

            goldText.text = $"Gold : {PlayerCharacter.Gold}";
            var playerHp = PlayerCharacter.Hp / PlayerCharacter.MaxHp;
            blood.fillAmount = playerHp;
            if (PlayerController.CanDash == false)
            {
                if (candash == true)
                {
                    Dash.fillAmount = 0;
                    DashCd = 0;
                    candash = false;
                }
            }
            
        }

        public void FixedUpdate()
        {
            if (candash == false)
            {
                DashCd += Time.deltaTime;
                Dash.fillAmount = DashCd / PlayerCharacter.DashCd;
                if (DashCd / PlayerCharacter.DashCd >= 1)
                {
                    StartCoroutine("SetDashCd");
                }
            }
        }

        public void Reloadative()
        {
            if (playerController.Ammo > 0)
            {
                StartCoroutine(Reload());
            }
        }
        
        IEnumerator Reload()
        {
            SoundManager.Instance.Play(SoundManager.Sound.Reload);
            foreach (var VARIABLE in Ammo)
            {
                VARIABLE.SetActive(false);
            }
            AmmoText.SetActive(true);
            yield return new WaitForSeconds(playerController.ReloadTime);
            AmmoText.SetActive(false);
            isReload = true;
        }
        
        public void OpenStatus()
        {
            if (StatusShow == false)
            {
                SetStatus();
                StatusUI.SetActive(true);
                StatusShow = true;
                Time.timeScale = 0;
                PlayerController.playerInput.Disable();
            }
            else
            {
                Back();
            }
            
        }

        private void Back()
        {
            StatusUI.SetActive(false);
            StatusShow = false;
            Time.timeScale = 1;
            PlayerController.playerInput.Enable();
        }

        IEnumerator SetDashCd()
        {
            yield return new WaitForSeconds(0.1f);
            candash = true;
            StopCoroutine(nameof(SetDashCd));
        }

        private void SetStatus()
        {
            MaxHPText.text = $"{PlayerCharacter.MaxHp}";
            SpeedText.text = $"{PlayerCharacter.Speed}";
            AtkText.text = $"{PlayerCharacter.Atk}";
            DashCdText.text = $"{PlayerCharacter.DashCd}";
            CritRateText.text = $"{PlayerCharacter.CritRate}";
            GoldText.text = $"{PlayerCharacter.Gold}";
        }
        
        public void Pause()
        {
            if (shopController.shoping == false)
            {
                //phoneUI.SetActive(false);
                PlayerController.playerInput.PlayerAction.Attack.Disable();
                PlayerController.playerInput.PlayerAction.Dash.Disable();
                PlayerController.playerInput.PlayerAction.Move.Disable();
                pauseUi.SetActive(true);
                Time.timeScale = 0;
                isPause = true;
            }
        }
        public void Resume()
        {
            pauseUi.SetActive(false);
            Time.timeScale = 1;
            isPause = false;
            PlayerController.playerInput.PlayerAction.Attack.Enable();
            PlayerController.playerInput.PlayerAction.Dash.Enable();
            PlayerController.playerInput.PlayerAction.Move.Enable();
        }

        private void Quit()
        {
            BackToHub?.PlayFeedbacks();
        }
        
        private void Restart()
        {
            PlayAgain?.PlayFeedbacks();
        }

        public void Dead()
        {
            var tolalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var VARIABLE in tolalEnemies)
            {
                VARIABLE.SetActive(false);
            }

            StartCoroutine(SetDead());
        }
        
        IEnumerator SetDead()
        {
            yield return new WaitForSeconds(1);
            pauseUi.SetActive(false);
            waveUI.SetActive(false);
            deadUI.SetActive(true);
            PlayerController.playerInput.PlayerAction.Disable();
            Time.timeScale = 0;
        }


    }
}
