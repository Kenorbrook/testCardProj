using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestGame
{
    [Serializable]
    [CreateAssetMenu(
        menuName = "Templates/Card library",
        fileName = "CardLibrary",
        order = 3)]
    public class CardLibrary : ScriptableObject
    {
        public string Name;
        public List<CardLibraryEntry> Entries = new();
    }
}
