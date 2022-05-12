using System;
using System.Collections;
using Script.Base;
using Script.Menu;
using Script.Sound;
using Script.Spawn;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.Controller
{
    public class ShopController : MonoBehaviour
    {
        [Header("Item")]
        [SerializeField] private Transform[]  spawnPoint;
        [SerializeField] private GameObject[] common;
        [SerializeField] private GameObject[] uncommon;
        [SerializeField] private GameObject[] rare;
        [SerializeField] private GameObject[] epic;
        [SerializeField] private GameObject[] cursed;
        [SerializeField] private GameObject[] item;
        [SerializeField] private GameObject shopMenu;
        [SerializeField] private GameObject ePopup;
        [SerializeField] private Button backButton;
        [SerializeField] private Button rngButton;
        [SerializeField] private Button healButton;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private TextMeshProUGUI healCostText;
        [SerializeField] private TextMeshProUGUI rngText;
        [Header("Cost")]
        [SerializeField] private int healCost = 20;
        [SerializeField] private int rngCost = 20;
        private bool shop = false;
        private Collider2D player;
        public bool shoping = false;
        
        
        public void Start()
        {
            PlayerController.playerInput.PlayerAction.Buy.performed += context => Talk();
            
            rngButton.onClick.AddListener(CheckGoldForReroll);
            backButton.onClick.AddListener(Back);
            healButton.onClick.AddListener(Heal);
        }

        public void Update()
        {
           
        }


        private void Talk()
        {
            if (shop == true)
            {
                SoundManager.Instance.Play(SoundManager.Sound.TalkWithShop);
                var talk = new[]
                {
                    "Hello , You come again","Oh, are you still alive?" , "HOW much money do you have " ,
                    "I think I saw your friend here." ,"if you get rich what do you do ",
                    "Hello World" ,"sometime we will meet again"
                };
                var range = Random.Range(0, talk.Length);
                shopMenu.SetActive(true);
                text.text = talk[range];
                healCostText.text = $"{healCost} Gold";
                rngText.text = $"{rngCost} Gold";
                shoping = true;
                PlayerController.playerInput.Disable();
            }
        }

        public void Back()
        {
            shopMenu.SetActive(false);
            shoping = false;
            PlayerController.playerInput.Enable();
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                player = other;
                shop = true;
                ePopup.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            shop = false;
            ePopup.SetActive(false);
        }

        public void CheckGoldForReroll()
        {
            var playerCharacter = player.GetComponent<PlayerCharacter>();
            if (playerCharacter.Gold >= rngCost)
            {
                playerCharacter.Gold -= rngCost;
                rngCost += 5;
                if (rngCost >= 50)
                {
                    rngCost = 50;
                }
                Deleteitem();
                RngItemandSpawn();
                text.text = $"Thank you for using the service. ";
                SoundManager.Instance.Play(SoundManager.Sound.ThankYou);
            }
            else
            {
                text.text = $"GOLD not enough";
                SoundManager.Instance.Play(SoundManager.Sound.NoMoney);
            }
        }

        public void Deleteitem()
        {
            var iteminscene = GameObject.FindGameObjectsWithTag("Item");
            foreach (var item in iteminscene)
            {
                Destroy(item);
            }
        }

        private void Heal()
        {
            var playerCharacter = player.GetComponent<PlayerCharacter>();
            if (playerCharacter.Gold >= healCost && playerCharacter.Hp < playerCharacter.MaxHp)
            {
                playerCharacter.Gold -= healCost;
                var heal50= (playerCharacter.MaxHp * 25)/100;
                playerCharacter.Hp += healCost;
                if (playerCharacter.Hp >= playerCharacter.MaxHp)
                {
                    playerCharacter.Hp = playerCharacter.MaxHp;
                }
                text.text = $"You gain hp +{heal50}. ";
                SoundManager.Instance.Play(SoundManager.Sound.ThankYou);
            }
            else if (playerCharacter.Hp >= playerCharacter.MaxHp)
            {
                text.text = $"Your blood is full  stop healing.";
                Debug.Log("wtf");
            }
            else
            {
                text.text = $"GOLD not enough.";
                SoundManager.Instance.Play(SoundManager.Sound.NoMoney);
            }
        }

       public void RngItemandSpawn()
        {
            foreach (var t in spawnPoint)
            {
                var rngTier = Random.Range(1 , 165);
                if (rngTier <= 68)
                {
                    var rngitem = Random.Range(0, common.Length);
                    Instantiate(common[rngitem], t.position ,Quaternion.identity);
                }
                else if (rngTier <= 114)
                {
                    var rngitem = Random.Range(0, common.Length);
                    Instantiate(uncommon[rngitem], t.position ,Quaternion.identity);
                }
                else if (rngTier <= 138)
                {
                    var rngitem = Random.Range(0, common.Length);
                    Instantiate(rare[rngitem], t.position ,Quaternion.identity);
                }
                else if (rngTier <= 149)
                {
                    var rngitem = Random.Range(0, common.Length);
                    Instantiate(epic[rngitem], t.position ,Quaternion.identity);
                }
                else if (rngTier <= 155)
                {
                    var rngitem = Random.Range(0, common.Length);
                    Instantiate(cursed[rngitem], t.position ,Quaternion.identity);
                }
                else if (rngTier <= 165)
                {
                    var rngitem = Random.Range(0, item.Length);
                    Instantiate(item[rngitem], t.position ,Quaternion.identity);
                }
            }
        }

        IEnumerator Test()
        {
            yield return new WaitForSeconds(3);
            var rngTier = Random.Range(1 , 156);
            if (rngTier <= 68)
            {
                Debug.Log($"rngTier = {rngTier}");
                Debug.Log("rngTier < 68");
            }
            else if (rngTier <= 114)
            {
                Debug.Log($"rngTier = {rngTier}");
                Debug.Log("rngTier < 114");
            }
            else if (rngTier <= 138)
            {
                Debug.Log($"rngTier = {rngTier}");
                Debug.Log("rngTier < 138");
            }
            else if (rngTier <= 149)
            {
                Debug.Log($"rngTier = {rngTier}");
                Debug.Log("rngTier < 149");
            }
            else if (rngTier <= 155)
            {
                Debug.Log($"rngTier = {rngTier}");
                Debug.Log("rngTier < 155");
            }
            StartCoroutine(Test());
        }
    }
}
