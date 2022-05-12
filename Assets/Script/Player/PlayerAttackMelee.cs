using System;
using Script.Sound;
using UnityEngine;

namespace Script.Player
{
    public class PlayerAttackMelee : MonoBehaviour
    {
        private PlayerAnimator playerAnimator;
        private bool isAttacking = false;
        private int attackTime = 0;
        private float attackCoolDowntime;
        private readonly float attackCoolDown = 2.5f;

        private void Start()
        {
            playerAnimator = GetComponent<PlayerAnimator>();
        }

        private void Update()
        {
            if (CheckAttackCoolDowntimeIsFinish())
            {
                ResetAttackTime();
                playerAnimator.ResetAnimationAttack();
            }
        }

        public void Attack(Vector3 playerInput)
        {
            if (isAttacking == false)
            {
                attackTime++;
                playerAnimator.SetAttackDirection(GetAttackDirection(playerInput));
                CheckAttackTime();
            }
        }

        private void CheckAttackTime()
        {
            switch (attackTime)
            {
                case 1:
                    FirstAttack();
                    break;
                case 2:
                    SecondAttack();
                    break;
                case 3:
                    ThirdAttack();
                    break;
            }
        }

        private void FirstAttack()
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.PlayerHit1);
            playerAnimator.PlayAnimationAttackTime(AttackTimeType.Attack01);
            SetIsAttacking();
            StartCountingAttackCoolDowntime();
        }

        private void SecondAttack()
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.PlayerHit2);
            playerAnimator.PlayAnimationAttackTime(AttackTimeType.Attack02);
            SetIsAttacking();
            StartCountingAttackCoolDowntime();
        }

        private void ThirdAttack()
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.PlayerHit3);
            playerAnimator.PlayAnimationAttackTime(AttackTimeType.Attack03);
            SetIsAttacking();
        }

        private void SetIsAttacking()
        {
            isAttacking = true;
        }

        private void StartCountingAttackCoolDowntime()
        {
            attackCoolDowntime = Time.time + attackCoolDown;
        }

        private void ResetAttackTime()
        {
            attackTime = 0;
            attackCoolDowntime = 0;
        }
        private Vector2 GetAttackDirection(Vector3 positionMouse)
        {
            return (positionMouse - transform.position).normalized;
        }
        
        private bool CheckAttackCoolDowntimeIsFinish()
        {
            return attackCoolDowntime <= Time.time;
        }
    }
}