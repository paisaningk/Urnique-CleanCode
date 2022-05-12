using System.Collections;
using Script.Base;
using UnityEngine;

namespace Script.Controller
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private float stoppingDistance = 3f;
        [SerializeField] private float movespeed = 20f;
        [SerializeField] private float Waitfornextmove = 3f;

        private EnemyCharacter enemyCharacter;
        private Rigidbody2D Rb;
        private Transform player;
        private Animator animator;
        private Vector3 directionnormalized;
        private bool attacking = true;
        private bool nextMove = true;

        private void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            enemyCharacter = GetComponent<EnemyCharacter>();
            player = GameObject.FindWithTag("Player").transform;
        }
        
        private void FixedUpdate()
        {
            if (enemyCharacter.isDeadForBoss == true)
            {
                animator.SetBool("Dead",true);
            }
            if (nextMove)
            {
                SelectNextMove();
            }
            
        }
        
        private void SelectNextMove()
        {
            var distance = Vector2.Distance(transform.position, player.position);
            var direction = player.position - transform.position;
            //var directionX = player.position.x - transform.position.x;
            //var directionY = player.position.y - transform.position.y;
            directionnormalized = direction.normalized;
            Debug.Log($"direction Y ={directionnormalized.y}");
            Debug.Log($"direction X ={directionnormalized.x}");
            
            
            if ( distance >= stoppingDistance)
            {
                //Debug.Log("walk");
                MoveCharacter(direction);
            }
            else if ( distance <= stoppingDistance)
            {
                if (attacking)
                {
                    Rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    animator.SetBool("Walking",false);
                    animator.SetBool("Attack",true);
                    animator.SetFloat("MoveX",directionnormalized.x);
                    animator.SetFloat("MoveY",directionnormalized.y);
                    attacking = false;
                    nextMove = false;
                }
                //Debug.Log("attacking");
            }
        }
        
        private void AttackFinish()
        {
            animator.SetBool("Walking",false);
            animator.SetBool("Attack",false);
            StartCoroutine(Wait3Sec());
        }

        private void Dead()
        {
            Destroy(this.gameObject);
        }
        
        IEnumerator Wait3Sec()
        {
            //SelectNextMove();
            yield return new WaitForSeconds(Waitfornextmove);
            Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            attacking = true;
            nextMove = true;
        }
        
        private void MoveCharacter(Vector3 direction)
        {
            Vector2 directionNormalized = direction.normalized;
            var move = (Vector2) transform.position + (directionNormalized * movespeed * Time.deltaTime);
            animator.SetFloat("MoveX",directionnormalized.x);
            animator.SetFloat("MoveY",directionnormalized.y);
            animator.SetBool("Walking",true);
            
            Rb.MovePosition(move);
        }
    }
}
