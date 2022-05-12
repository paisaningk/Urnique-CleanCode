using System;
using UnityEngine;

namespace Script.Save
{
    [System.Serializable]
    public class SaveData : MonoBehaviour
    {
        public bool Wave;
        public int Killednow;
        public bool a;
        
        public static SaveData Instance { get; private set; }
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
