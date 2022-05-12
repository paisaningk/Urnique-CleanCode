using Script.Base;
using Script.Sound;
using UnityEngine;

namespace Script.Pickup
{
    public class Gold : MonoBehaviour
    {
        public int goldAmount;

        private void OnTriggerEnter2D(Collider2D other)
        {
            
            if (other.CompareTag("Player"))
            {
                SoundManager.Instance.Play(SoundManager.Sound.Coin);
                AddGold(other,goldAmount);
            }
        }

        public void AddGold(Collider2D other ,int goldPlus)
        {
            var player = other.GetComponent<PlayerCharacter>();
            player.Gold += goldPlus;

            Destroy(gameObject);
        }
    }
}
