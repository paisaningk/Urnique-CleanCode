using System;
using Script.Sound;
using UnityEngine;

namespace Script.Controller
{
    public class PlayerControllerOutsideScene : MonoBehaviour
    {
        [SerializeField]private float moveSpeed;
        public static Playerinput PlayerInput;
        public Playerinput playerInput;
        private Rigidbody2D rd;
        private Vector2 moveDie;
        private Animator animator;
        private bool soundplay;

        void Awake()
        {
            PlayerInput = new Playerinput();
            playerInput = new Playerinput();
            animator = GetComponent<Animator>();
            rd = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            SoundManager.Instance.Play(SoundManager.Sound.PlayerMovement);
            PlayerInput.Enable();
        }
        
        void Update()
        {
            var walk = PlayerInput.PlayerAction.Move.ReadValue<Vector2>();
            moveDie = walk.normalized;
            
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
            rd.velocity = moveDie * moveSpeed;
        }
        
        private void FixedUpdate()
        {
            
        }
    }
}
