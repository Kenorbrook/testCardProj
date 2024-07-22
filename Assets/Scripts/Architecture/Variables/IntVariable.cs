using UnityEngine;

namespace TestGame
{
    [CreateAssetMenu(
        menuName = "Architecture/Variables/Integer",
        fileName = "Variable",
        order = 0)]
    public class IntVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = string.Empty;
#endif

        public int Value;

        public GameEventInt ValueChangedEvent;

        internal void SetValue(int value)
        {
            Value = value; 
            ValueChangedEvent?.Raise(value);
        }
    }
}