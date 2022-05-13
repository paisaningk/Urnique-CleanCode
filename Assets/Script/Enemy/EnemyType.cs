using System;
using Script.Enemy.Finite_State_Machine;
using UnityEngine;

namespace Script.Enemy
{
    public class EnemyType : MonoBehaviour
    {
        protected EnemyBaseState.NameBot nameBot;
        protected EnemyBaseState currentState;
        protected GameObject botGameObject;
        protected float starMove;
        protected float startMoveSlowest;
        protected new Rigidbody2D rigidbody;
        protected Transform playerTransform;
        protected Animator animator;
        protected float moveSpeed;
        private void Start()
        {
            SetupComponent();
            SetUpState();
        }

        private void Update()
        {
            StateProcess();
        }

        private void StateProcess()
        {
            currentState.Process();
        }
        private void SetUpState()
        {
            currentState = new Move(botGameObject, playerTransform, nameBot, starMove, startMoveSlowest, rigidbody,
                animator, moveSpeed);
        }

        private void SetupComponent()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }
    }
}