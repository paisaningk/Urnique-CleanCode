using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    [Serializable]
    public class ObjectPoolItem 
    {
        public int amountToPool;
        public GameObject objectToPool;
        public bool shouldExpand;
    }
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool SharedInstance;
        public List<ObjectPoolItem> ItemsToPool;
        private List<GameObject> pooledObjects;

        void Awake() 
        {
            SharedInstance = this;
        }

        private void Start()
        {
            pooledObjects = new List<GameObject>();
            foreach (var item in ItemsToPool) 
            {
                for (var i = 0; i < item.amountToPool; i++) 
                {
                    var obj = Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                }
            }
        }

        public GameObject GetPooledObject(string tag) 
        {
            foreach (var item in pooledObjects)
            {
                if (!item.activeInHierarchy && item.CompareTag(tag)) 
                {
                    return item;
                }
            }
            foreach (ObjectPoolItem item in ItemsToPool) 
            {
                if (item.objectToPool.CompareTag(tag))
                {
                    if (item.shouldExpand)
                    {
                        GameObject obj = (GameObject) Instantiate(item.objectToPool);
                        obj.SetActive(false);
                        pooledObjects.Add(obj);
                        return obj;
                    }
                }
            }
            return null;
        }
    }
}