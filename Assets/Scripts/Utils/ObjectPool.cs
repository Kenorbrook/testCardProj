using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TestGame
{

    public class ObjectPool<T, U> : MonoBehaviour where T :MonoBehaviour where U : PooledObject<T>
    {
        public T Prefab;
        public int InitialSize;

        private readonly Stack<U> instances = new();
        private readonly List<U> objectsToReturn = new();

        private readonly List<U> childrenComponents = new(32);

        private void Awake()
        {
            Assert.IsNotNull(Prefab);
        }

        internal void Initialize()
        {
            for (var i = 0; i < InitialSize; i++)
            {
                U obj = CreateInstance();
                obj.gameObject.SetActive(false);
                instances.Push(obj);
            }
        }

        internal T GetObject()
        {
            U pooledObject = instances.Count > 0 ? instances.Pop() : CreateInstance();
            pooledObject.gameObject.SetActive(true);
            return pooledObject.PooledObj;
        }

        internal void ReturnObject(U obj)
        {
            Assert.IsNotNull(obj);

            obj.gameObject.SetActive(false);
            if (!instances.Contains(obj))
            {
                instances.Push(obj);
            }
        }

        internal void Reset()
        {
            objectsToReturn.Clear();

            transform.GetComponentsInChildren(false, childrenComponents);
            foreach (var obj in childrenComponents)
            {
                objectsToReturn.Add(obj);
            }

            foreach (var instance in objectsToReturn)
            {
                ReturnObject(instance);
            }
        }

        private U CreateInstance()
        {
            T obj = Instantiate(Prefab, transform, true);
            U pooledObject = obj.gameObject.AddComponent<U>();
            pooledObject.PooledObj = obj;
            return pooledObject;
        }
    }
}
