using System;
using Script.Base;
using Script.Player;
using UnityEngine;

namespace Script.UI
{
    public class PlayerDate : MonoBehaviour
    {
        [SerializeField] private GameObject[] players;
        internal PlayerCharacter playerCharacter;
        internal PlayerMovement playerMovement;
        internal PlayerAttackMelee playerAttackMelee;
        internal PlayerAttackRanged playerAttackRanged;

        private void Awake()
        {
            SetupComponent();
        }
        
        private void SetupComponent()
        {
            foreach (var player in players)
            {
                if (player.activeInHierarchy)
                {
                    playerCharacter = player.GetComponent<PlayerCharacter>();
                    playerMovement = player.GetComponent<PlayerMovement>();
                    playerAttackMelee = player.GetComponent<PlayerAttackMelee>();
                    playerAttackRanged = player.GetComponent<PlayerAttackRanged>();
                }
            }
        }
    }
}