using System;
using Script.Base;
using UnityEngine;

namespace Script
{
    public class Bullet : MonoBehaviour
    {
        public int Atk;
        public int CritRate;
        public int CritAtk;
        public new Renderer renderer;

        public void OnEnable()
        {
            var a = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>();
            Atk = a.Atk;
            CritAtk = a.CritAtk;
            CritRate = a.CritRate;
        }

        public void Update()
        {
            if (!renderer.isVisible) 
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            { 
                gameObject.SetActive(false);
            }
        }
    }
}
