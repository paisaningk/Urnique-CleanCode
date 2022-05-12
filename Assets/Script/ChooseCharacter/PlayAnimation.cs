using System;
using Script.Controller;
using Script.Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class PlayAnimation : MonoBehaviour
    {
        public ChooseCharacter ChooseCharacter;
        public Animator Animator;
        public PlayerType PlayerType;
        public GameObject Ui;


        private void OnMouseEnter()
        {
            if (!ChooseCharacter.IsSelect)
            {
                Animator.SetBool("select",true);
                Ui.SetActive(true);
            }
        }

        private void OnMouseExit()
        {
            if (!ChooseCharacter.IsSelect)
            {
                Animator.SetBool("select",false);
                Ui.SetActive(false);   
                
            }
            
        }
    }
}
