using System.Collections;
using Script.Sound;
using UnityEngine;

namespace Script.Controller
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float StarMove = 1.5f;
        [SerializeField] private float StarMoveslowe = 2f;
        private Rigidbody2D rb;
        private Transform player;
        private Animator animator;
        private float movespeed = 8f;
        private float stoppingDistance = 1.9f;
        private Vector3 directionnormalized;
        private bool attacking = false;
        private bool nextMove = false;
        

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            player = GameObject.FindWithTag("Player").transform;
        }

        private void FixedUpdate()
        {
            if (nextMove == false)
            {
                Selectnextmove();
            }
            
        }
        
        private void Playsound()
        {
            SoundManager.Instance.Play(SoundManager.Sound.TankAttack);
        }
        
        private void Playsound02()
        {
            SoundManager.Instance.Play(SoundManager.Sound.EyeAttack);
        }
    
        private void MoveCharacter(Vector3 direction)
        {
            Vector2 directionNormalized = direction.normalized;
            var move = (Vector2) transform.position + (directionNormalized * movespeed * Time.deltaTime);
            
            animator.SetFloat("MoveX",directionnormalized.x);
            animator.SetFloat("MoveY",directionnormalized.y);
            animator.SetBool("Walking",true);
            
            rb.MovePosition(move);
        }
        
        IEnumerator Wait()
        {
            var a = Random.Range(StarMove,StarMoveslowe);
            yield return new WaitForSeconds(a);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            attacking = false;
            nextMove = false;
        }

        private void AttackFinish()
        {
            animator.SetBool("Attack",false);
            StartCoroutine(Wait());
        }

        private void Selectnextmove()
        {
            var distance = Vector2.Distance(transform.position, player.position);
            var direction = player.position - transform.position;
            directionnormalized = direction.normalized;
            
            if ( distance >= stoppingDistance)
            {
                MoveCharacter(direction);
            }
            else if ( distance <= stoppingDistance)
            {
                if (attacking == false)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    animator.SetBool("Walking",false);
                    animator.SetBool("Attack",true);
                    animator.SetFloat("MoveX",directionnormalized.x);
                    animator.SetFloat("MoveY",directionnormalized.y);
                    nextMove = true;
                    attacking = true;
                }
            }
        }
    }
}
