using System.Collections.Generic;
using UnityEngine;

namespace TestGame
{
    public abstract class BaseSystem : MonoBehaviour
    {
        protected CharacterObject Player;
        protected List<CharacterObject> Enemy; 

        public void Initialize(CharacterObject player, List<CharacterObject> enemy)
        {
            Player = player;
            Enemy = enemy;
        }
    }
}