using UnityEngine;

namespace Script.Popup
{
    public class Popup : MonoBehaviour
    {
        private float destroyTime = 3f;
        private Vector3[] offSet = new[] {new Vector3(0, 1f, 0),
            new Vector3(0.5f,1f,0),new Vector3(-0.5f,1f,0)};
    
        void Start()
        {
            Destroy(this.gameObject , destroyTime);
            int randomNumber = Random.Range(0,3);
            var offsetrandom = offSet[randomNumber];
            transform.localPosition += offsetrandom;
        }

        // Update is called once per frame
    
    }
}
