using System;
using UnityEngine;

namespace TestGame
{
    [Serializable]
    [CreateAssetMenu(
        menuName = "Templates/Player",
        fileName = "Player",
        order = 1)]
    public class PlayerTemplate : CharacterTemplate
    {
        public int Hp;
        public int Mana;
        public CardLibrary StartingDeck;
    }
}
