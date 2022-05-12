using System.Collections;
using Script.Sound;
using Script.Spawn;
using UnityEngine;

namespace Script.Controller
{
    public enum AttackState
    {
        Attack,
        Ring,
    }
    public class RealBossController : MonoBehaviour
    {
        [SerializeField] private float StarMove = 3f;
        [SerializeField] private float StarMoveslowe = 4f;
        private Rigidbody2D rb;
        private Transform player;
        public Animator BodyAnimator;
        public Animator ArmAnimator;
        public Animator RingAnimator;
        private float movespeed = 5f;
        private float stoppingDistance = 3f;
        private bool nextMove = false;
        private bool selectNextAttack;
        private AttackState attackState;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindWithTag("Player").transform;
            if (SpawnPlayer.instance.PlayerType == PlayerType.Gun)
            {
                movespeed = 7.5f;
            }
        }

        private void FixedUpdate()
        {
            if (nextMove == false)
            {
                SelectNextMove();
            }
            var direction = (player.position - transform.position).normalized;
            transform.localScale = direction.x < 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        }

        private void MoveCharacter(Vector3 direction)
        {
            Vector2 directionNormalized = direction.normalized;
            var move = (Vector2) transform.position + (directionNormalized * movespeed * Time.deltaTime);
            BodyAnimator.SetBool("Walk",true);
            ArmAnimator.SetBool("Walk",true);
            rb.MovePosition(move);
        }
        
        IEnumerator Wait()
        {
            var a = Random.Range(StarMove,StarMoveslowe);
            yield return new WaitForSeconds(a);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            nextMove = false;
            selectNextAttack = true;
        }

        public void AttackFinish()
        {
            BodyAnimator.SetBool("Walk",false);
            ArmAnimator.SetBool("Walk",false);
            BodyAnimator.SetBool("Attack",false);
            ArmAnimator.SetBool("Attack",false);
            StartCoroutine(Wait());
        }
        
        public void RingFinish()
        {
            BodyAnimator.SetBool("Walk",false);
            ArmAnimator.SetBool("Walk",false);
            BodyAnimator.SetBool("Skill",false);
            ArmAnimator.SetBool("Skill",false);
            RingAnimator.SetBool("Skill",false);
            StartCoroutine(Wait());
        }

        public void StartRing()
        {
            RingAnimator.SetBool("Skill",true);
        }
        
        public void Sound01()
        {
            SoundManager.Instance.Play(SoundManager.Sound.BossAttack01);
        }
        
        public void Sound02()
        {
            SoundManager.Instance.Play(SoundManager.Sound.BossAttack02);
        }
        

        private void SelectNextMove()
        {
            var distance = Vector2.Distance(transform.position, player.position);
            var direction = player.position - transform.position;
            if (selectNextAttack)
            {
                var random = Random.Range(1,3);
                attackState = random == 1 ? AttackState.Attack : AttackState.Ring;
                selectNextAttack = false;
            }

            if (distance >= stoppingDistance)
            {
                MoveCharacter(direction);
            }
            else if (distance <= stoppingDistance)
            {
                if (attackState == AttackState.Attack)
                {
                    BodyAnimator.SetBool("Attack",true);
                    ArmAnimator.SetBool("Attack",true);
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    nextMove = true;
                }
                else
                {
                    BodyAnimator.SetBool("Skill",true);
                    ArmAnimator.SetBool("Skill",true);
                    RingAnimator.SetBool("Skill",true);
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    nextMove = true;
                }
                
            }
        }

    }
}
