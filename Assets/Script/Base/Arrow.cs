using System;
using Script.Controller;
using Script.Spawn;
using UnityEngine;

namespace Script.Base
{
    public class Arrow : MonoBehaviour
    {
        public float speed;
        public int DMG;

        private Transform player;
        private Vector2 target;

        private PlayerController playerController;
        private bool updatePlayer = false;

        private void Start()
        {
            player = GameObject.FindWithTag("Player").transform;
        }
        
    
        void Update()
        {
            if (updatePlayer == false)
            {
                target = new Vector2(player.position.x, player.position.y);
                updatePlayer = true;
            }
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (transform.position.x == target.x && transform.position.y == target.y)
            {
                gameObject.SetActive(false);
                updatePlayer = false;
            }
        
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PlayerHitBox"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}

    
