using UnityEngine;

namespace Script.Enemy.Finite_State_Machine
{
    public class Move : EnemyBaseState
    {
        public Move(GameObject botGameObject, Transform playerTransform, NameBot nameBot, float starMove, float startMoveSlowest, Rigidbody2D rigidbody, Animator animator, float moveSpeed) : base(botGameObject, playerTransform, nameBot, starMove, startMoveSlowest, rigidbody, animator, moveSpeed)
        {
        }
        
        protected override void Update()
        {
            MoveCharacter();
            CheckDistancePlayerToSwitchState();
        }


        private void CheckDistancePlayerToSwitchState()
        {
            if (GetDistancePlayer() <= 2)
            {
                nextState = new Attack(botGameObject, playerTransform, nameBot, starMove, startMoveSlowest, rigidbody,
                    animator, moveSpeed);
            }
        }

        private void MoveCharacter()
        {
            PlayAnimationWalk();
            rigidbody.MovePosition(GetMovePosition());
        }

        private void PlayAnimationWalk()
        {
            animator.SetFloat("MoveX",GetDirectionNormalized().x);
            animator.SetFloat("MoveY",GetDirectionNormalized().y);
            animator.SetBool("Walking",true);
        }
    }
}