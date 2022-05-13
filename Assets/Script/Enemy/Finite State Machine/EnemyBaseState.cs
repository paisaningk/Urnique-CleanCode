using UnityEngine;

namespace Script.Enemy.Finite_State_Machine
{
    public class EnemyBaseState
    {
        protected enum EvenState
      {
         Enter, Update, Exit
      }

      public enum NameBot
      {
         Sword,   
         Executioner,
         Boss
      }
      
      protected EnemyBaseState nextState;
      
      protected NameBot nameBot;
      protected EvenState evenState;
      protected GameObject botGameObject;
      protected float starMove;
      protected float startMoveSlowest;
      protected Rigidbody2D rigidbody;
      protected Transform playerTransform;
      protected Animator animator;
      protected float moveSpeed;

      public EnemyBaseState(GameObject botGameObject, Transform playerTransform,NameBot nameBot, float starMove, float startMoveSlowest, Rigidbody2D rigidbody, Animator animator, float moveSpeed)
      {
         this.botGameObject = botGameObject;
         this.nameBot = nameBot;
         this.playerTransform  = playerTransform;
         this.starMove = starMove;
         this.startMoveSlowest = startMoveSlowest;
         this.rigidbody = rigidbody;
         this.animator = animator;
         this.moveSpeed = moveSpeed;
         this.evenState = EvenState.Enter;
      }


      protected virtual void Enter()
      {
         evenState = EvenState.Update;
      }

      protected virtual void Update()
      {
         evenState = EvenState.Update;
      }

      protected virtual void Exit()
      {
         evenState = EvenState.Exit;
      }
   
      public EnemyBaseState Process()
      {
         if (evenState == EvenState.Exit)
         {
            Exit();
            return nextState;
         }
         if (evenState == EvenState.Enter)
         {
            Enter();
         }
         if (evenState == EvenState.Update)
         {
            Update();
            CheckName(nameBot);
         }
         return this;
      }

      protected void CheckName(NameBot namebot)
      {
         if (namebot == NameBot.Sword)
         {
            IsSword();
         }
         else if (namebot == NameBot.Executioner)
         {
            IsExecutioner();
         }
         else if (namebot == NameBot.Boss)
         {
            IsBoss();
         }
      }

      protected virtual void IsSword()
      {
         evenState = EvenState.Exit;
      }
      
      protected virtual void IsExecutioner()
      {
         evenState = EvenState.Exit;
      }
      
      protected virtual void IsBoss()
      {
         evenState = EvenState.Exit;
      }

      protected float GetDistancePlayer()
      {
         return Vector3.Distance(botGameObject.transform.position,playerTransform.transform.position);
      }

      protected Vector3 GetDirection()
      {
        return playerTransform.position - botGameObject.transform.position;
      }
      
      protected Vector3 GetDirectionNormalized()
      {
         return (playerTransform.position - botGameObject.transform.position).normalized;
      }

      protected Vector3 GetMovePosition()
      {
         return  botGameObject.transform.position + (GetDirectionNormalized() * moveSpeed * Time.deltaTime);
      }
      
      
      
    }
}