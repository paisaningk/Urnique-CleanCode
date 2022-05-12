using Script.Sound;
using UnityEngine;

namespace Script.Base
{
    public class EnemyArchor : MonoBehaviour
    {
        public float speed;
        public float stoppingDistance;
        public float retreatDistance;
        public float StartShot = 1.5f;
        public float EndShot = 2.5f;
        private Rigidbody2D rb;
        private Animator animator;
        private Transform player;
        private bool attacking = true;
        private float timeBtwShots;

        public GameObject projectile;

        void Start()
        {
            animator = GetComponent<Animator>();
            player = GameObject.FindWithTag("Player").transform;
            rb = GetComponent<Rigidbody2D>();
            var random = Random.Range(StartShot,EndShot);
            timeBtwShots = random;
        }

        void FixedUpdate()
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (attacking)
            {
                if ( distance > stoppingDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.position
                        , speed * Time.deltaTime);
                }

                else if (distance < stoppingDistance && distance > retreatDistance)
                {
                    transform.position = this.transform.position;
                }

                else if (distance < retreatDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.position
                        , -speed * Time.deltaTime);
                }

                if(timeBtwShots <= 0)
                {
                    animator.SetBool("Attack",true);
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    attacking = false;
                }
            }

            timeBtwShots -= Time.deltaTime;
        }

        private void Shots()
        {
            SoundManager.Instance.Play(SoundManager.Sound.WitchAttack);
            var arrow = ObjectPool.SharedInstance.GetPooledObject("Projectile");
            arrow.SetActive(true);
            arrow.transform.position = transform.position;
            var random = Random.Range(StartShot,EndShot);
            timeBtwShots = random;
        }

        private void AnimatorFinish()
        {
            attacking = true;
            animator.SetBool("Attack",false);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
