using UnityEngine;

namespace TestGame
{
    public class FloatingObject : MonoBehaviour
    {
        public float Amplitude = 0.1f;
        public float Frequency = 0.75f;

        private Vector3 pos;

        private void Start()
        {
            pos = transform.position;
        }

        private void Update()
        {
            var newPos = pos;
            newPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * Frequency) * Amplitude;
            transform.position = newPos;
        }
    }
}
