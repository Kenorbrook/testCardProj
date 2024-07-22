using System.Collections.Generic;
using UnityEngine;

namespace TestGame
{
    
    [CreateAssetMenu(menuName = "Create Pattern", fileName = "Pattern", order = 0)]
    public class Pattern : ScriptableObject
    {
        public List<IntegerEffect> Effects = new();
        public bool isRandomPattern;
    }
}
