using UnityEngine;

namespace TestGame
{
    public class PooledObject<T> : MonoBehaviour where T: MonoBehaviour
    {
        internal T PooledObj;
    }
}