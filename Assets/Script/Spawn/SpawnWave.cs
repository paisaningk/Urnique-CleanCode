using System;
using System.Collections;
using MoreMountains.Feedbacks;
using Script.Base;
using Script.Controller;
using Script.Pickup;
using Script.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace Script.Spawn
{
    [System.Serializable]
    public class Wave
    {
        public string WaveName;
        public int numberOfEnemy;
        public float spawnTime;
        public GameObject[] typeOfEnemy;

    }
    public class SpawnWave : MonoBehaviour
    {
        [Header("Wave")]
        [SerializeField] private Wave[] Ezmode;
        [SerializeField] private Wave[] WaveForPlayerGun;
        [SerializeField] private Transform[] SpawnPoint;
        [SerializeField] private TextMeshProUGUI WaveText;
        [SerializeField] private GameObject Shop;
        [SerializeField] private GameObject win;
        [SerializeField] private float shopingtime = 120;
        [SerializeField] private TextMeshProUGUI Nextwavetext;
        [SerializeField] private GameObject nextwaveGameObject;
        [SerializeField] private GameObject PlayerSword;
        [SerializeField] private GameObject PlayerGun;
        [SerializeField] private Camera CameraMap;
        [SerializeField] private Camera CameraShop;
        [SerializeField] private Transform MapPoint;
        [SerializeField] private Transform ShopPoint;
        [SerializeField] private GameObject SkipText;
        [SerializeField] private GameObject UiBoss;
        [SerializeField] private Image blood;
        [SerializeField] private ShopController shop;
        public MMFeedbacks Fade;
        public MMFeedbacks Fade2;
        private Wave CurrentWave;
        private Wave[] Wave;
        private int CurrentWaveNumber = 0;
        public bool CountTimeNextWave = false;
        private bool nextwave = true;
        private bool CanSpawn = true;
        private bool soundPlay = true;
        private float nextSpawnTime;
        private int WaveNumberText = 1;
        private float timeShopShow;
        private ShopController shopController;
        private bool canSpawn = false;
        private GameObject Player;
        private PlayerCharacter playerCharacter;
        private bool skip = false;
        private EnemyCharacter HPboss;

        private void Awake()
        {
            if (SpawnPlayer.instance.PlayerType == PlayerType.Gun) 
            {
                PlayerGun.SetActive(true);
            }
            else
            {
                PlayerSword.SetActive(true);
            }

            Wave = SpawnPlayer.instance.Mode == Mode.Easy ? Ezmode : WaveForPlayerGun;
            PlayerController.playerInput.PlayerAction.Skip.performed += context =>  Close();
            UiBoss.SetActive(false);
        }
        private void Start()
        {
            SkipText.SetActive(false);
            WaveNumberText = 1;
            WaveText.text = $"Wave {WaveNumberText}";
            Shop.SetActive(false);
            timeShopShow = shopingtime;
            StartCoroutine(Wait());
            Player = GameObject.FindWithTag("Player");
            playerCharacter = Player.GetComponent<PlayerCharacter>();
        }

        private void Close()
        {
            if (CountTimeNextWave && skip)
            {
                StopCoroutine("Shoping");
                StartCoroutine("WaitFade2");
            }
        }

        private void Update()
        {
            if (HPboss == null) return;
            var bossHp = HPboss.Hp / HPboss.MaxHp;
            blood.fillAmount = bossHp;
        }

        private void FixedUpdate()
        {
            if (canSpawn)
            {
                if (CurrentWaveNumber <= Wave.Length)
                {
                    CurrentWave = Wave[CurrentWaveNumber];
                    spawnWave();
                    var tolalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                    WaveText.text = $"Wave : {WaveNumberText}";
                    if (tolalEnemies.Length == 0 && !CanSpawn && CurrentWaveNumber + 1 >= Wave.Length)
                    {
                        win.SetActive(true);
                        UiBoss.SetActive(false);
                        PlayerController.playerInput.PlayerAction.Disable();
                    }
                    else if (tolalEnemies.Length == 0 && !CanSpawn && CurrentWaveNumber + 1 != Wave.Length)
                    {
                        if (nextwave)
                        {
                            StartCoroutine( "WaitFade");
                            var coin = GameObject.FindGameObjectsWithTag("Coin");
                            Debug.Log(coin.Length);
                            foreach (var gold in coin)
                            {
                                playerCharacter.Gold += gold.GetComponent<Gold>().goldAmount;;
                                Destroy(gold);
                            }
                            nextwave = false;
                            timeShopShow = shopingtime;
                        }
                    }
                    else if (tolalEnemies.Length == 0 && !CanSpawn)
                    {
                        CurrentWaveNumber++;
                    }
                }
                if (CountTimeNextWave)
                {
                    if (soundPlay)
                    {
                        SoundManager.Instance.Stop(SoundManager.Sound.BGM);
                        SoundManager.Instance.Play(SoundManager.Sound.Shop);
                        soundPlay = false;
                    }
                    nextwaveGameObject.SetActive(true);
                    var a = (int) (timeShopShow -= Time.deltaTime);
                    Nextwavetext.text = $"Next Wave in coming in {a} Sec";
                }
            }
        }
        
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(1.5f);
            canSpawn = true;
        }
        
        IEnumerator WaitFade()
        {
            Fade.PlayFeedbacks();
            yield return new WaitForSeconds(1);
            StartCoroutine( "Shoping");
        }
        IEnumerator WaitFade2()
        {
            Fade2.PlayFeedbacks();
            skip = false;
            yield return new WaitForSeconds(1);
            CloseShop();
        }

        private void spawnWave()
        {
            if (CanSpawn && nextSpawnTime < Time.time)
            {
                SoundManager.Instance.Play(SoundManager.Sound.SpawnEnemy);
                var RandomEnemy = CurrentWave.typeOfEnemy[Random.Range(0, CurrentWave.typeOfEnemy.Length)];
                var RandomSpawnPoint = SpawnPoint[Random.Range(0, SpawnPoint.Length)];
                Instantiate(RandomEnemy, RandomSpawnPoint.position, Quaternion.identity);
                CurrentWave.numberOfEnemy--;
                nextSpawnTime = Time.time + CurrentWave.spawnTime;
                if (CurrentWave.numberOfEnemy == 0)
                {
                    CanSpawn = false;
                    var tolalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (var VARIABLE in tolalEnemies)
                    {
                        var b = VARIABLE.GetComponent<EnemyCharacter>();
                        if (b.isBoss == true)
                        {
                            HPboss = b;
                            UiBoss.SetActive(true);
                            SoundManager.Instance.Stop(SoundManager.Sound.BGM);
                            SoundManager.Instance.Play(SoundManager.Sound.BGMBoss);
                        }
                    }
                }
            }
        }

        private IEnumerator Shoping()
        {
            OpenShop();

            yield return new WaitForSeconds(shopingtime);

            StartCoroutine(WaitFade2());

        }

        private void OpenShop()
        {
            Player.transform.position = ShopPoint.position;
            CameraMap.gameObject.SetActive(false);
            CameraShop.gameObject.SetActive(true);
            SoundManager.Instance.Play(SoundManager.Sound.OpenShop);
            shopController = Shop.GetComponent<ShopController>();
            Shop.SetActive(true);
            shopController.RngItemandSpawn();
            CountTimeNextWave = true;
            SkipText.SetActive(true);
            skip = true;
        }

        private void CloseShop()
        {
            shop.Back();
            skip = false;
            CameraMap.gameObject.SetActive(true);
            CameraShop.gameObject.SetActive(false);
            Shop.SetActive(false);
            shopController.Deleteitem();
            NextSpawnWave();
            CountTimeNextWave = false;
            nextwaveGameObject.SetActive(false);
            SkipText.SetActive(false);
            Player.transform.position = MapPoint.position;
        }

        private void NextSpawnWave()
        {
            nextwave = true;
            WaveNumberText++;
            CurrentWaveNumber++;
            CanSpawn = true;
            soundPlay = true;
            SoundManager.Instance.Stop(SoundManager.Sound.Shop);
            SoundManager.Instance.Playfrompause(SoundManager.Sound.BGM);
        }
    }
}
