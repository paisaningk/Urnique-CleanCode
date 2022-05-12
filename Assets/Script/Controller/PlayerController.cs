using System.Collections;
using Script.Base;
using Script.Menu;
using Script.Sound;
using UnityEngine;

namespace Script.Controller
{
    public enum PlayerType 
    {
        SwordMan, Gun
    }
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask dashLayerMask;
        [SerializeField] public GameplaySceneMenu GameplaySceneMenu;
        [SerializeField] public PlayerType playerType;
        private PlayerCharacter playerCharacter;
        public static Playerinput playerInput;
        private Rigidbody2D Rd;
        private Vector3 MoveDie;
        private Animator animator;
        private Camera cam;
        private bool IsAttacking = false;
        private bool Attack01 = false;
        private bool Attack02 = false;
        private bool Attack03 = false;
        [SerializeField] private Transform Gun;
        [SerializeField] private GunController gunController;
        [SerializeField] private Animator Gunanimator;
        public static bool CanDash = true;
        public bool knockback = false;
        public bool fire = false;
        public bool canfire = true;
        

        //ปรับได้
        private float MoveSpeed = 5f;
        float dashAmount = 3f;
        private float dashcooldown = 1;
        private float Attackcooldowntime;
        private float Attackcooldown = 2.5f;
        private bool Soundplay;
        public int Ammo;
        public float ReloadTime = 2;
        private bool FireRateCoolDown = true;

        private void Awake()
        {
            Time.timeScale = 1f;
            animator = GetComponent<Animator>();
            Rd = GetComponent<Rigidbody2D>();
            playerCharacter = GetComponent<PlayerCharacter>();
            playerInput = new Playerinput();
            playerInput.PlayerAction.Dash.performed += context => Dash();
            playerInput.PlayerAction.Pause.performed += context => Menu();
            playerInput.PlayerAction.Cheat.performed += context => Cheat();
            if (playerType == PlayerType.SwordMan)
            {
                playerInput.PlayerAction.Attack.performed += context => Attack();
            }
            else
            {
                playerInput.PlayerAction.Attack.performed += context => Shot();
                playerInput.PlayerAction.Reload.performed += context => AllReload();
                gunController = GetComponent<GunController>();
            }
            OnEnable();
        }
        private void Start()
        {
            SoundManager.Instance.Play(SoundManager.Sound.PlayerMovement);
            cam = Camera.main;
        }

        private void Cheat()
        {
            playerCharacter.Gold += 100;
            playerCharacter.MaxHp += 100;
            playerCharacter.Hp += 100;
        }

        private void Update()
        {
            dashcooldown = playerCharacter.DashCd;
            MoveSpeed = playerCharacter.Speed;
            //Walk
            var walk = playerInput.PlayerAction.Move.ReadValue<Vector2>();
            MoveDie = walk.normalized;
            
            if (playerType == PlayerType.Gun)
            {
                GunFollowMouse();
                if (Ammo == 5)
                {
                    StartCoroutine(ResetAmmo());
                    StartCoroutine(Reload());
                }
            }

            if (walk != Vector2.zero)
            {
                if (Soundplay)
                {
                    SoundManager.Instance.Play(SoundManager.Sound.PlayerMovement);
                    Soundplay = false;
                }
                animator.SetFloat("MoveX",walk.x);
                animator.SetFloat("MoveY",walk.y);
                animator.SetBool("Walking",true);
            }
            else
            {
                SoundManager.Instance.Stop(SoundManager.Sound.PlayerMovement);
                Soundplay = true;
                animator.SetBool("Walking",false); 
            }

            if (Attackcooldowntime <= Time.time)
            {
                AttackFinish03();
            }
        }

        private void FixedUpdate()
        {
            Rd.velocity = MoveDie * MoveSpeed;
        }

        private void Menu()
        {
            if (GameplaySceneMenu.isPause == false)
            {
                GameplaySceneMenu.Pause();
            }
            else if (GameplaySceneMenu.isPause == true)
            {
                GameplaySceneMenu.Resume();
            }
        }

        private void AllReload()
        {
            if (Ammo > 0)
            {
                StartCoroutine(ResetAmmo());
                StartCoroutine(Reload());
            }
        }

        private void Shot()
        {
            if (FireRateCoolDown)
            {
                fire = true;
            }
        }

        private void GunFollowMouse()
        {
            Vector2 mousePosition = playerInput.PlayerAction.Mouse.ReadValue<Vector2>();
            var a = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
            var mouse = cam.ScreenToWorldPoint(a);
            
            Vector2 difference = mouse - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Gun.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

            if (canfire && fire)
            {
                Gunanimator.SetBool("Fire",true);
                StartCoroutine(FireRate());
                Ammo++;
                fire = false;
                float distance = difference.magnitude;
                Vector2 direction = difference / distance;
                direction.Normalize();
                gunController.FireBullet(direction, rotationZ,Ammo);
            }
        }
        
        IEnumerator FireRate()
        {
            FireRateCoolDown = false;
            yield return new WaitForSeconds(0.2f);
            Gunanimator.SetBool("Fire",false);
            FireRateCoolDown = true;
        }

        IEnumerator Reload()
        {
            Gunanimator.SetBool("Reload",true);
            fire = false;
            canfire = false;
            yield return new WaitForSeconds(ReloadTime);
            Gunanimator.SetBool("Reload",false);
            canfire = true;
            fire = false;
        }
        
        IEnumerator ResetAmmo()
        {
            yield return new WaitForSeconds(0.1f);
            Ammo = 0;
        }
        

        private void Attack()
        {
            if (IsAttacking == false)
            {
                var positionMouse = cam.ScreenToWorldPoint(playerInput.PlayerAction.Mouse.ReadValue<Vector2>());
                Vector3 vectorAttack = (positionMouse - transform.position).normalized;
                animator.SetFloat("AttackX",vectorAttack.x);
                animator.SetFloat("AttackY",vectorAttack.y);

                if (Attack01 == false)
                {
                    SoundManager.Instance.Play(SoundManager.Sound.PlayerHit1);
                    IsAttacking = true;
                    Attack01 = true;
                    animator.SetBool("Attacking",true); 
                    animator.SetBool("Attack01",true);
                    Attackcooldowntime = Time.time + Attackcooldown;
                    
                }
                else if (Attack02 == false)
                {
                    SoundManager.Instance.Play(SoundManager.Sound.PlayerHit2);
                    IsAttacking = true;
                    Attack02 = true;
                    animator.SetBool("Attacking",true);
                    animator.SetBool("Attack02",true);
                    Attackcooldowntime = Time.time + Attackcooldown;
                    //Debug.Log($"Attack 2");
                }
                else if (Attack03 == false)
                {
                    SoundManager.Instance.Play(SoundManager.Sound.PlayerHit3);
                    IsAttacking = true;
                    Attack03 = true;
                    animator.SetBool("Attacking",true); 
                    animator.SetBool("Attack03",true);
                    knockback = true;
                    //Debug.Log($"Attack 3");
                }
            }
        }
        
        private void Dash()
        {
            if (CanDash && MoveDie != Vector3.zero)
            {
                CanDash = false;
                SoundManager.Instance.Play(SoundManager.Sound.PlayerDash);
                Vector3 dashPoint = transform.position + MoveDie * dashAmount;

                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, MoveDie, 
                    dashAmount,dashLayerMask);
                if (raycastHit2D.collider != null)
                {
                    dashPoint = raycastHit2D.point;
                }
                Rd.MovePosition(dashPoint);
                StartCoroutine(DashCooldown());
            }
        }

        IEnumerator DashCooldown()
        {
            yield return new WaitForSeconds(dashcooldown);
            CanDash = true;
        }

        private void AttackFinish()
        {
            animator.SetBool("Attacking",false);
            IsAttacking = false;
        }
        
        public void AttackFinish03()
        {
            IsAttacking = false;
            Attack01 = false;
            Attack02 = false;
            Attack03 = false;
            animator.SetBool("Attacking",false);
            animator.SetBool("Attack01",false);
            animator.SetBool("Attack02",false);
            animator.SetBool("Attack03",false);
            Attackcooldowntime = 0;
        }

        public void Dead()
        {
            SoundManager.Instance.Play(SoundManager.Sound.PlayerDie);
            GameplaySceneMenu.Dead();
            Rd.constraints = RigidbodyConstraints2D.FreezeAll;
            OnDisable();
        }

        public void OnEnable()
        {
            playerInput.Enable();
        }

        public void OnDisable()
        {
            playerInput.Disable();
        }
        
    }
}
