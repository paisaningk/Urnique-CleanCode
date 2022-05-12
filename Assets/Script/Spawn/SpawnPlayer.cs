using Script.Controller;
using UnityEngine;

namespace Script.Spawn
{
    public class SpawnPlayer : MonoBehaviour
    {
        public static SpawnPlayer instance;
        public PlayerType PlayerType;
        public Mode Mode;
        public int Item = 0;
        public int Monster = 0;
        
        void Awake()
        {
            if (instance != null && instance != this)
                Destroy(this.gameObject);
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}
