using System.Collections;
using Assets.scriptableobject.Item;
using MoreMountains.Feedbacks;
using Script.Controller;
using Script.Sound;
using scriptableobject.Character;
using UnityEngine;

namespace Script.Base
{
    public class PlayerCharacter : MonoBehaviour 
    {
        [SerializeField] private CharacterSO PlayerCharacterSo;
        [Header("Player Status")]
        [SerializeField] public string Name;
        [SerializeField] public float MaxHp;
        [SerializeField] public float Hp;
        [SerializeField] public int Atk;
        [SerializeField] public int Gold = 0;
        [SerializeField] public float Speed;
        [SerializeField] public float DashCd;
        [SerializeField] public int CritAtk = 1;
        [SerializeField] public int CritRate = 2;
        public MMFeedbacks PlayerHit;
        public PlayerType PlayerType;
        public ItemSO[] ItemSo;

        private GameObject Popup;
        private Animator animator;
        private PlayerController playerController;
        private SpriteRenderer spriteRenderer;
        private bool isTakedmg = false;

        public void Awake()
        {
            Name = PlayerCharacterSo.Name;
            Hp = PlayerCharacterSo.MaxHp;
            MaxHp = PlayerCharacterSo.MaxHp;
            Atk = PlayerCharacterSo.Atk;
            Speed = PlayerCharacterSo.Speed;
            Popup = PlayerCharacterSo.Popup;
            animator = GetComponent<Animator>();
            playerController = GetComponent<PlayerController>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (PlayerType == PlayerType.SwordMan)
            {
                DashCd = 3.5f;
            }
            else
            {
                DashCd = 5;
            }
        }

        public void PrintAll()
        {
            Debug.Log($"name:{Name}");
            Debug.Log($"HP:{Hp}");
            Debug.Log($"ATK:{Atk}");
            Debug.Log($"Speed:{Speed}");
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isTakedmg == false)
            {
                if (other.CompareTag("EnemyHitBox"))
                {
                    PlayerHit?.PlayFeedbacks();
                    SoundManager.Instance.Play(SoundManager.Sound.PlayerTakeHit);
                    var enemyCharacter = other.GetComponentInParent<EnemyCharacter>();
                    Hp -= enemyCharacter.Atk;
                    StartCoroutine(Setcoloattack());
                    ShowPopUp(enemyCharacter.Atk);
                    isTakedmg = true;

                    if (Hp <= 0)
                    {
                        animator.SetBool("Dead", true);
                        playerController.Dead();
                    }
                }

                if (other.CompareTag("Projectile"))
                {
                    PlayerHit?.PlayFeedbacks();
                    SoundManager.Instance.Play(SoundManager.Sound.PlayerTakeHit);
                    var arrow = other.GetComponent<Arrow>();
                    Hp -= arrow.DMG;
                    ShowPopUp(arrow.DMG);
                    StartCoroutine(Setcoloattack());
                    isTakedmg = true;
                    //other.gameObject.SetActive(false);
                    if (Hp <= 0)
                    {
                        animator.SetBool("Dead", true);
                        playerController.Dead();
                    }
                }
            }
        }

        IEnumerator Blockdmg()
        {
            if (PlayerType == PlayerType.SwordMan)
            {
                spriteRenderer.color = Color.blue;
            }
            else
            {
                spriteRenderer.color = Color.green;
            }
            yield return new WaitForSeconds(0.5f);
            isTakedmg = false;
            spriteRenderer.color = Color.white;
        }
        
        IEnumerator Setcoloattack()
        { 
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(Blockdmg());
        }

        private void ShowPopUp(int dmg)
        {
            var spawnPopup = Instantiate(Popup,transform.position,Quaternion.identity,transform);
            var textMesh = spawnPopup.GetComponent<TextMesh>();
            textMesh.text = $"{dmg}";
            textMesh.color = Color.red;
        }

        private void Dead()
        {
            SoundManager.Instance.Stop(SoundManager.Sound.BGM);
            SoundManager.Instance.Play(SoundManager.Sound.PlayerDieBGM);
            Time.timeScale = 0;
        }
    }
}
