using System.Collections;
using Script.Base;
using Script.Menu;
using Script.Player;
using Script.Sound;
using UnityEngine;
using UnityEngine.Playables;

namespace Script.Controller
{
    public enum PlayerType 
    {
        SwordMan, Gun
    }
    public class PlayerControllerOld : MonoBehaviour
    {
        [SerializeField] private LayerMask dashLayerMask;
        [SerializeField] public GameplaySceneMenuOld gameplaySceneMenu;
        [SerializeField] public PlayerType playerType;
        private PlayerCharacter playerCharacter;
        public static Playerinput playerInput;
        private Rigidbody2D rd;
        private Vector3 moveDie;
        private Animator animator;
        private Camera cam;
        private bool isAttacking = false;
        private bool attack01 = false;
        private bool attack02 = false;
        private bool attack03 = false;
        [SerializeField] private Transform gun;
        [SerializeField] private GunController gunController;
        [SerializeField] private Animator gunanimator;
        public static bool canDash = true;
        public bool knockback = false;
        public bool fire = false;
        public bool canfire = true;
        

        //ปรับได้
        private float moveSpeed = 5f;
        float dashAmount = 3f;
        private float dashcooldown = 1;
        private float attackcooldowntime;
        private float attackcooldown = 2.5f;
        private bool soundplay;
        public int ammo;
        public float reloadTime = 2;
        private bool fireRateCoolDown = true;

        private void Awake()
        {
            Time.timeScale = 1f;
            animator = GetComponent<Animator>();
            rd = GetComponent<Rigidbody2D>();
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
            moveSpeed = playerCharacter.Speed;
            //Walk
            var walk = playerInput.PlayerAction.Move.ReadValue<Vector2>();
            moveDie = walk.normalized;
            
            if (playerType == PlayerType.Gun)
            {
                GunFollowMouse();
                if (ammo == 5)
                {
                    StartCoroutine(ResetAmmo());
                    StartCoroutine(Reload());
                }
            }

            if (walk != Vector2.zero)
            {
                if (soundplay)
                {
                    SoundManager.Instance.Play(SoundManager.Sound.PlayerMovement);
                    soundplay = false;
                }
                animator.SetFloat("MoveX",walk.x);
                animator.SetFloat("MoveY",walk.y);
                animator.SetBool("Walking",true);
            }
            else
            {
                SoundManager.Instance.Stop(SoundManager.Sound.PlayerMovement);
                soundplay = true;
                animator.SetBool("Walking",false); 
            }

            if (attackcooldowntime <= Time.time)
            {
                AttackFinish03();
            }
        }

        private void FixedUpdate()
        {
            rd.velocity = moveDie * moveSpeed;
        }

        private void Menu()
        {
            if (gameplaySceneMenu.isPause == false)
            {
                gameplaySceneMenu.Pause();
            }
            else if (gameplaySceneMenu.isPause == true)
            {
                gameplaySceneMenu.Resume();
            }
        }

        private void AllReload()
        {
            if (ammo > 0)
            {
                StartCoroutine(ResetAmmo());
                StartCoroutine(Reload());
            }
        }

        private void Shot()
        {
            if (fireRateCoolDown)
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
            gun.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

            if (canfire && fire)
            {
                gunanimator.SetBool("Fire",true);
                StartCoroutine(FireRate());
                ammo++;
                fire = false;
                float distance = difference.magnitude;
                Vector2 direction = difference / distance;
                direction.Normalize();
                gunController.FireBullet(direction, rotationZ,ammo);
            }
        }
        
        IEnumerator FireRate()
        {
            fireRateCoolDown = false;
            yield return new WaitForSeconds(0.2f);
            gunanimator.SetBool("Fire",false);
            fireRateCoolDown = true;
        }

        IEnumerator Reload()
        {
            gunanimator.SetBool("Reload",true);
            fire = false;
            canfire = false;
            yield return new WaitForSeconds(reloadTime);
            gunanimator.SetBool("Reload",false);
            canfire = true;
            fire = false;
        }
        
        IEnumerator ResetAmmo()
        {
            yield return new WaitForSeconds(0.1f);
            ammo = 0;
        }
        

        private void Attack()
        {
            if (isAttacking == false)
            {
                var positionMouse = cam.ScreenToWorldPoint(playerInput.PlayerAction.Mouse.ReadValue<Vector2>());
                Vector3 vectorAttack = (positionMouse - transform.position).normalized;
                animator.SetFloat("AttackX",vectorAttack.x);
                animator.SetFloat("AttackY",vectorAttack.y);

                if (attack01 == false)
                {
                    SoundManager.Instance.Play(SoundManager.Sound.PlayerHit1);
                    isAttacking = true;
                    attack01 = true;
                    animator.SetBool("Attacking",true); 
                    animator.SetBool("Attack01",true);
                    attackcooldowntime = Time.time + attackcooldown;
                    
                }
                else if (attack02 == false)
                {
                    SoundManager.Instance.Play(SoundManager.Sound.PlayerHit2);
                    isAttacking = true;
                    attack02 = true;
                    animator.SetBool("Attacking",true);
                    animator.SetBool("Attack02",true);
                    attackcooldowntime = Time.time + attackcooldown;
                    //Debug.Log($"Attack 2");
                }
                else if (attack03 == false)
                {
                    SoundManager.Instance.Play(SoundManager.Sound.PlayerHit3);
                    isAttacking = true;
                    attack03 = true;
                    animator.SetBool("Attacking",true); 
                    animator.SetBool("Attack03",true);
                    knockback = true;
                    //Debug.Log($"Attack 3");
                }
            }
        }
        
        private void Dash()
        {
            if (canDash && moveDie != Vector3.zero)
            {
                canDash = false;
                SoundManager.Instance.Play(SoundManager.Sound.PlayerDash);
                Vector3 dashPoint = transform.position + moveDie * dashAmount;

                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, moveDie, 
                    dashAmount,dashLayerMask);
                if (raycastHit2D.collider != null)
                {
                    dashPoint = raycastHit2D.point;
                }
                rd.MovePosition(dashPoint);
                StartCoroutine(DashCooldown());
            }
        }

        IEnumerator DashCooldown()
        {
            yield return new WaitForSeconds(dashcooldown);
            canDash = true;
        }

        private void AttackFinish()
        {
            animator.SetBool("Attacking",false);
            isAttacking = false;
        }
        
        public void AttackFinish03()
        {
            isAttacking = false;
            attack01 = false;
            attack02 = false;
            attack03 = false;
            animator.SetBool("Attacking",false);
            animator.SetBool("Attack01",false);
            animator.SetBool("Attack02",false);
            animator.SetBool("Attack03",false);
            attackcooldowntime = 0;
        }

        public void Dead()
        {
            SoundManager.Instance.Play(SoundManager.Sound.PlayerDie);
            gameplaySceneMenu.Dead();
            rd.constraints = RigidbodyConstraints2D.FreezeAll;
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
