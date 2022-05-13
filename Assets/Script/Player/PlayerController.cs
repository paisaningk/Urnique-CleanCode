using System.Collections;
using Script.Base;
using Script.Controller;
using Script.Menu;
using Script.Sound;
using UnityEngine;

namespace Script.Player
{
    public enum PlayerType 
    {
        SwordMan, Gun
    }
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] public PlayerType playerType;
        
        private PlayerMovement playerMovement;
        private PlayerAnimator playerAnimator;
        private PlayerAttackMelee playerAttackMelee;
        private PlayerAttackRanged playerAttackRanged;
        public static Playerinput playerInput;
        
        private void Awake()
        {
            SetupComponents();
            SetControllerForPlayerType();
            OnPlayerInputEnable();
        }

        private void Update()
        {
            playerMovement.SetMoveDirectionNormalized(GetMoveDirection());
            CheckMoveDirectionToPlayAnimationWalk();
            CheckIsPlayerTypeGunToExecute();
        }

        private void SetupComponents()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerAnimator = GetComponent<PlayerAnimator>();
            playerAttackMelee = GetComponent<PlayerAttackMelee>();
            if (IsPlayerTypeGun())
            {
                playerAttackRanged = GetComponent<PlayerAttackRanged>();
            }
        }
        
        private void CheckIsPlayerTypeGunToExecute()
        {
            if (!IsPlayerTypeGun()) return;
            playerAttackRanged.GunFollowMouse();
            playerAttackRanged.SetMousePosition(GetMouseInScreenToWorldPoint());
        }

        private void CheckMoveDirectionToPlayAnimationWalk()
        {
            if (playerMovement.CheckMoveDirectionEqualVector3Zero())
            {
                playerAnimator.PlayAnimationWalk(playerMovement.moveDirection);
            }
            else
            {
                playerAnimator.StopAnimationWalk();
            }
        }

        private bool IsPlayerTypeGun()
        {
            return playerType == PlayerType.Gun;
        }

        private void SetControllerForPlayerType()
        {
            playerInput = new Playerinput();
            playerInput.PlayerAction.Dash.performed += context => playerMovement.Dash();

            if (IsPlayerTypeGun())
            {
                playerInput.PlayerAction.Attack.performed += context => playerAttackRanged.ShootBullet();
                playerInput.PlayerAction.Reload.performed += context => playerAttackRanged.CheckCanReload();
            }
            else
            {
                playerInput.PlayerAction.Attack.performed += context => 
                    playerAttackMelee.Attack(GetMouseDirection());
            }
        }

        private Vector3 GetMouseInScreenToWorldPoint()
        {
            return Camera.main.ScreenToWorldPoint(playerInput.PlayerAction.Mouse.ReadValue<Vector3>());
        }

        private Vector3 GetMouseDirection()
        {
            return playerInput.PlayerAction.Mouse.ReadValue<Vector3>();
        }
        
        private Vector3 GetMoveDirection()
        {
            return playerInput.PlayerAction.Move.ReadValue<Vector3>();
        }

        public void OnPlayerInputEnable()
        {
            playerInput.Enable();
        }

        public void OnPlayerInputDisable()
        {
            playerInput.Disable();
        }
        
    }
}
