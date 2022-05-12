using System;
using System.Collections;
using Script.Base;
using Script.Sound;
using UnityEngine;

namespace Script.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private LayerMask dashLayerMask;
        
        public Vector3 moveDirection;
        
        private PlayerCharacter playerCharacter;
        private new Rigidbody2D rigidbody;
        private float moveSpeed = 5f;
        private bool isSoundPlaying;
        private float dashAmount = 3f;
        private float dashCooldown = 1;
        public static bool canDash = true;

        public void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            playerCharacter = GetComponent<PlayerCharacter>();
        }

        public void Update()
        {
            SetMoveSpeed();

            if (CheckMoveDirectionEqualVector3Zero())
            {
                PlaySoundWalk();
            }
            else
            {
                StopSoundWalk();
            }
        }
        
        private void FixedUpdate()
        {
            Walk();
        }

        public void Walk()
        {
            rigidbody.velocity = moveDirection * moveSpeed;
        }
        
        private void SetMoveSpeed()
        {
            moveSpeed = playerCharacter.Speed;
        }
        
        public void SetMoveDirectionNormalized(Vector3 direction)
        {
            moveDirection =  direction.normalized;
        }

        private void PlaySoundWalk()
        {
            if (isSoundPlaying)
            {
                SoundManager.Instance.PlaySound(SoundManager.Sound.PlayerMovement);
                isSoundPlaying = false;
            }
        }

        private void StopSoundWalk()
        {
            SoundManager.Instance.StopSound(SoundManager.Sound.PlayerMovement);
            isSoundPlaying = true;
        }

        public bool CheckMoveDirectionEqualVector3Zero()
        {
            return moveDirection != Vector3.zero;
        }

        public void Dash()
        {
            if (IsCanDash())
            {
                canDash = false;
                SoundManager.Instance.PlaySound(SoundManager.Sound.PlayerDash);
                
                rigidbody.MovePosition(IsHitWall());
                StartCoroutine(nameof(StartDashCooldown));
            }
        }

        private Vector3 GetDashPoint()
        {
            return transform.position + moveDirection * dashAmount;
        }

        private Vector2 IsHitWall()
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, moveDirection, 
                dashAmount,dashLayerMask);
            if (raycastHit2D.collider != null)
            {
                 return raycastHit2D.point;
            }

            return GetDashPoint();
        }

        private bool IsCanDash()
        {
            return canDash && CheckMoveDirectionEqualVector3Zero();
        }

        private IEnumerator StartDashCooldown()
        {
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }
}