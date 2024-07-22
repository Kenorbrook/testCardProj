using System.Collections.Generic;
using UnityEngine;

namespace TestGame
{
    [CreateAssetMenu(
        menuName = "Architecture/Events/Game event",
        fileName = "GameEvent",
        order = 0)]
    public class GameEvent : ScriptableObject
    {
        private readonly List<GameEventListener> listeners = new();

        public void Raise()
        {
            for (var i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaised();
        }

        internal void RegisterListener(GameEventListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        internal void UnregisterListener(GameEventListener listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }
    }
}