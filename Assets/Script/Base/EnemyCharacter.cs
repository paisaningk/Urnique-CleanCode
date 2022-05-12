using System;
using System.Collections;
using MoreMountains.Feedbacks;
using Script.Controller;
using Script.Pickup;
using Script.Save;
using Script.Sound;
using Script.Spawn;
using scriptableobject.Character;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Base
{
    public class EnemyCharacter : MonoBehaviour
    {
        [SerializeField] public CharacterSO EnemyCharacterSo;
        [SerializeField] private GameObject GoldPrefab;
        [SerializeField] private GameObject Monster;
        [SerializeField] private EnemyType enemyType;
        public bool isBoss;
        [SerializeField] private GameObject Popup;
        public MMFeedbacks SlowTime;
        public MMFeedbacks Cam;
        private string Name;
        public float MaxHp;
        public float Hp;
        public int Atk;
        public float Speed;
        private Rigidbody2D Rb;
        private PlayerCharacter player;
        private float playerCritRate;
        private Vector3 offset;
        [SerializeField] private SpriteRenderer spriteRenderer;
        public bool isDeadForBoss = false;
        private bool CanHit = false;


        public void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>();
            SetSO();
        }
        
        private void SetSO()
        {
            Name = EnemyCharacterSo.Name;
            MaxHp = EnemyCharacterSo.MaxHp;
            Hp = EnemyCharacterSo.MaxHp;
            Atk = EnemyCharacterSo.Atk;
            Speed = EnemyCharacterSo.Speed;
            Popup = EnemyCharacterSo.Popup;
            Rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        public void PrintAll()
        {
            Debug.Log("Enemy");
            Debug.Log($"name:{Name}");
            Debug.Log($"HP:{Hp}");
            Debug.Log($"ATK:{Atk}");
            Debug.Log($"Speed:{Speed}");
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PlayerHitBox"))
            {
                if (CanHit == false)
                {
                    SoundManager.Instance.Play(SoundManager.Sound.Hit);
                    Cam?.PlayFeedbacks();
                    StartCoroutine(CanAttack());
                    var atkPlayer = other.GetComponentInParent<PlayerCharacter>();
                    playerCritRate = atkPlayer.CritRate;
                    var critPercentRand = Random.Range(1, 101);
                
                    if (critPercentRand <= playerCritRate)
                    {
                        var atkCrit = atkPlayer.Atk * atkPlayer.CritAtk;
                        ShowPopUpCrit(atkCrit);
                        Hp -= atkCrit;
                        StartCoroutine(Setcoloattack());
                    }
                    else
                    {
                        ShowPopUp(atkPlayer.Atk);
                        Hp -= atkPlayer.Atk;
                        StartCoroutine(Setcoloattack());
                    }
                
                    if (Hp <= 0)
                    {
                        SoundManager.Instance.Play(SoundManager.Sound.EnemyTakeHit);
                        StartCoroutine(Deaddelay());
                        SlowTime?.PlayFeedbacks();
                    }
                }
            }
            else if (other.CompareTag("Bullet"))
            {
                SoundManager.Instance.Play(SoundManager.Sound.Hit);
                Cam?.PlayFeedbacks();
                var atkPlayer = other.GetComponent<Bullet>();
                playerCritRate = atkPlayer.CritRate;
                var critPercentRand = Random.Range(1, 101);
                
                if (critPercentRand <= playerCritRate)
                {
                    var atkCrit = atkPlayer.Atk * atkPlayer.CritAtk;
                    ShowPopUpCrit(atkCrit);
                    Hp -= atkCrit;
                    StartCoroutine(Setcoloattack());
                }
                else
                {
                    ShowPopUp(atkPlayer.Atk);
                    Hp -= atkPlayer.Atk;
                    StartCoroutine(Setcoloattack());
                }
                
                if (Hp <= 0)
                {
                    SlowTime?.PlayFeedbacks();
                    SoundManager.Instance.Play(SoundManager.Sound.EnemyTakeHit);
                    SoundManager.Instance.Play(SoundManager.Sound.Die);
                    StartCoroutine(Deaddelay());
                    
                }
            }
        }
        
        IEnumerator CanAttack()
        {
            CanHit = true;
            yield return new WaitForSeconds(0.2f);
            CanHit = false;
        }

        IEnumerator Setcoloattack()
        {
            var a = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = a;
        }

        IEnumerator Deaddelay()
        {
            DropGold();
            yield return new WaitForSeconds(0.1f);
            Destroy(this.gameObject);
        }

        private void Knockback(Collider2D other)
        {
            var knockbackForce = 300;
            Vector2 difference = (Rb.transform.position - other.transform.position).normalized;
            Vector2 force = difference * knockbackForce;
            var raycastHit2D = Physics2D.Raycast(transform.position,difference,knockbackForce);
            if (raycastHit2D.collider != null) force = raycastHit2D.point;
            Rb.AddForce(force,ForceMode2D.Impulse);
        }
        
        private void ShowPopUp(int dmg)
        {
            var spawnPopup = Instantiate(Popup,transform.position,Quaternion.identity,transform);
            var textMesh = spawnPopup.GetComponent<TextMesh>();
            textMesh.text = $"{dmg}";
            textMesh.color = Color.white;
        }
        
        private void ShowPopUpCrit(int dmg)
        {
            var spawnPopup = Instantiate(Popup,transform.position,Quaternion.identity,transform);
            var textMesh = spawnPopup.GetComponent<TextMesh>();
            textMesh.text = $"{dmg}";
            textMesh.color = Color.yellow;
        }

        private void DropGold()
        {
            var gold = 0;
            switch (enemyType)
            {
                case EnemyType.Slime:
                    gold = Random.Range(5 , 15);
                    gold /= 2;
                    GoldPrefab.GetComponent<Gold>().goldAmount = gold;
                    break;
                case EnemyType.Ranger:
                    gold = Random.Range(7 , 12);
                    GoldPrefab.GetComponent<Gold>().goldAmount = gold;
                    break;
                case EnemyType.Golem:
                    gold = Random.Range(8 , 12);
                    GoldPrefab.GetComponent<Gold>().goldAmount = gold;
                    break;
                case EnemyType.Charger:
                    gold = Random.Range(12 , 18);
                    GoldPrefab.GetComponent<Gold>().goldAmount = gold;
                    break;
                case EnemyType.Boss:
                    gold = Random.Range(35 , 41);
                    GoldPrefab.GetComponent<Gold>().goldAmount = gold;
                    break;
            }
            Instantiate(GoldPrefab,transform.position, Quaternion.identity);
            var DropMonster = Random.Range(1, 100);
            if (DropMonster >= 50)
            {
                Instantiate(Monster,transform.position + new Vector3(2,0), Quaternion.identity);
            }
        }
    }
}
