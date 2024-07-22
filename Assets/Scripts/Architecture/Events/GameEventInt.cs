using System.Collections.Generic;
using UnityEngine;

namespace TestGame
{
    [CreateAssetMenu(
        menuName = "Architecture/Events/Game event (int)",
        fileName = "GameEvent",
        order = 1)]
    public class GameEventInt : ScriptableObject
    {
        private readonly List<GameEventIntListener> listeners = new();

        internal void Raise(int value)
        {
            for (var i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaised(value);
        }

        internal void RegisterListener(GameEventIntListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        internal void UnregisterListener(GameEventIntListener listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }
    }
}