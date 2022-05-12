using System.Collections;
using Script.Controller;
using UnityEngine;

namespace Script
{
    public class AnimationGun : MonoBehaviour
    {
        public Animator animator;
        private static readonly int Pew = Animator.StringToHash("PEW");
        
        void Start()
        {
            PlayerController.playerInput.PlayerAction.Attack.performed += context => SetAttack();
        }

        void SetAttack()
        {
            animator.SetBool(Pew,true);
            StartCoroutine(WaitForSeconds());
        }
    
        IEnumerator WaitForSeconds()
        {
            yield return new WaitForSeconds(1f);
            animator.SetBool(Pew,false);
            StopCoroutine(WaitForSeconds());
        }
    }
}
