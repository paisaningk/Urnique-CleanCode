
using Script.Sound;
using Script.Spawn;
using UnityEngine;

public class MonsterPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
            
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.Play(SoundManager.Sound.Monster);
            SpawnPlayer.instance.Monster++;
            Destroy(gameObject);
        }
    }
}
