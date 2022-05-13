using System.Collections;
using UnityEngine;

namespace Script.Enemy.Finite_State_Machine
{
    public class Attack : EnemyBaseState
    {
        private float timeStamp;
        public Attack(GameObject botGameObject, Transform playerTransform, NameBot nameBot, float starMove, float startMoveSlowest, Rigidbody2D rigidbody, Animator animator, float moveSpeed) : base(botGameObject, playerTransform, nameBot, starMove, startMoveSlowest, rigidbody, animator, moveSpeed)
        {
            timeStamp = Time.time + 10;
        }

        protected override void Enter()
        {
            StopWalk();
            PlayAnimationAttack();
            
        }

        protected override void Update()
        {
            CheckCoolDown();
        }

        private void PlayAnimationAttack()
        {
            animator.SetBool("Walking",false);
            animator.SetBool("Attack",true);
            animator.SetFloat("MoveX",GetDirectionNormalized().x);
            animator.SetFloat("MoveY",GetDirectionNormalized().y);
        }

        private void StopWalk()
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void CheckCoolDown()
        {
            if (CoolDownFinish())
            {
                SwitchState();
            }
        }

        private bool CoolDownFinish()
        {
            return timeStamp < Time.time;
        }

        private void SwitchState()
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            nextState = new Move(botGameObject, playerTransform, nameBot, starMove, startMoveSlowest, rigidbody,
                animator, moveSpeed);
        }
    }
}