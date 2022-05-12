using System;
using Assets.scriptableobject.Item;
using Script.Base;
using Script.Controller;
using Script.Menu;
using Script.Sound;
using Script.Spawn;
using UnityEngine;

namespace Script.Pickup
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemSO ItemSo;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private GameObject Popup;
        [SerializeField] private GameObject buy;
        [SerializeField] private GameObject text;
        private bool Buying = false;
        private int maxHp;
        private int atk;
        private float speed;
        private float dashCd;
        private int critAtk;
        private int critRate;
        private int price;
        private Tier tier;
        private Collider2D player;

        private void Start()
        {
            maxHp = ItemSo.MaxHp;
            atk = ItemSo.Atk;
            speed = ItemSo.Speed;
            dashCd = ItemSo.DashCd;
            critAtk = ItemSo.CritAtk;
            critRate = ItemSo.CritRate;
            tier = ItemSo.Tier;
            sprite.sprite = ItemSo.Sprite;
            var textMesh = text.GetComponent<TextMesh>();
            textMesh.text = $"{ItemSo.text}";
            ShowPrice();
            PlayerController.playerInput.PlayerAction.Buy.performed += context => Buy();
            
            //price = ItemSo.Price;
            //PrintAll();
        }
        


        private void PrintAll()
        {
            Debug.Log($"MaxHp {maxHp}");
            Debug.Log($"ATK {atk}");
            Debug.Log($"Speed {speed}");
            Debug.Log($"DashCd {dashCd}");
            Debug.Log($"CritAtk {critAtk}");
            Debug.Log($"critRate {critRate}");
            Debug.Log($"tier {tier}");
        }
        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                player = other;
                Buying = true;
                buy.SetActive(Buying);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Buying = false;
            //ShopController.BuyingPhone.SetActive(Buying);
            buy.SetActive(Buying);
            
        }

        private void ShowPrice()
        {
            switch (tier)
            {
                case Tier.Common:
                    price = 20;
                    break;
                case Tier.Uncommon:
                    price = 30;
                    break;
                case Tier.Rare:
                    price = 40;
                    break;
                case Tier.Epic:
                    price = 50;
                    break;
                case Tier.Cursed:
                    price = 100;
                    break;
                case Tier.ItemForNPC:
                    price = 35;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var textMesh = Popup.GetComponent<TextMesh>();
            textMesh.text = $"{price}";
        }

        private void PickUp(Collider2D other)
        {
            if (tier ==  Tier.ItemForNPC)
            {
                SpawnPlayer.instance.Item++;
                Debug.Log(SpawnPlayer.instance.Item);
            }
            var Player = other.GetComponent<PlayerCharacter>();
            Player.MaxHp += maxHp;
            Player.Hp += maxHp;
            Player.Atk += atk;
            Player.Speed += speed;
            Player.DashCd -= dashCd;
            Player.CritAtk += critAtk;
            Player.CritRate += critRate;
            Debug.Log("Player pickup");

            if (Player.DashCd < 0.5)
            {
                Player.DashCd = 0.5f;
            }

            if (Player.Speed > 8)
            {
                Player.Speed = 8;
            }
            SoundManager.Instance.Play(SoundManager.Sound.Pickup);
            Destroy(gameObject);
        }

        private void Buy()
        {
            if (Buying)
            {
                var playerGold = player.GetComponent<PlayerCharacter>().Gold;
                if (playerGold >= price)
                {
                    player.GetComponent<PlayerCharacter>().Gold -= price;
                    PickUp(player);
                }
            }
            else
            {
                //เดี่ยวใส่เสียง
                Debug.Log("can't pick up");
            }
        }
    }
}
