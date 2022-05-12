using System.Collections;
using UnityEngine;

namespace Script.Spawn
{
    public class SpawnItem : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Time.timeScale = 0.7f;
                StartCoroutine(Wait3sec());
            }
        }

        IEnumerator Wait3sec()
        {
            yield return new WaitForSeconds(3);
            Time.timeScale = 1;
            Debug.Log("it back");
        }
    }
}
