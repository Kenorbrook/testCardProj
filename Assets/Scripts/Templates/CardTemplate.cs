using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestGame
{
    [Serializable]
    [CreateAssetMenu(
        menuName = "Templates/Card",
        fileName = "Card",
        order = 0)]
    public class CardTemplate : ScriptableObject
    {
        public int Id;
        public string Name;
        public int Cost;
        public Material Material;
        public Sprite Picture;
        public List<Effect> Effects = new();
    }
}
