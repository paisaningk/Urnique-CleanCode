using UnityEngine;

namespace Script.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Animator animator;
        private float attackCoolDowntime = 0;
        private float attackCoolDown = 2.5f;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayAnimationWalk(Vector3 moveDirection)
        {
            animator.SetFloat("MoveX",moveDirection.x);
            animator.SetFloat("MoveY",moveDirection.y);
            animator.SetBool("Walking",true);
        }

        public void StopAnimationWalk()
        {
            animator.SetBool("Walking",false); 
        }
        
        //Using in unity
        private void AttackIsFinish()
        {
            animator.SetBool("Attacking",false);
        }
        
        //Using in unity
        public void ResetAnimationAttack()
        {
            animator.SetBool("Attacking",false);
            animator.SetBool("Attack01",false);
            animator.SetBool("Attack02",false);
            animator.SetBool("Attack03",false);
        }

        public void PlayAnimationAttackTime(AttackTimeType attackTimeType)
        {
            animator.SetBool("Attacking",true); 
            animator.SetBool($"{attackTimeType}",true);
        }

        public void SetAttackDirection(Vector2 attackDirection)
        {
            animator.SetFloat("AttackX",attackDirection.x);
            animator.SetFloat("AttackY",attackDirection.y);
        }
    }
}