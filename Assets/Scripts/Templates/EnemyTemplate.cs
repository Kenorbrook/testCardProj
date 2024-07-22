using System;
using UnityEngine;

namespace TestGame
{
    [Serializable]
    [CreateAssetMenu(
        menuName = "Templates/Enemy",
        fileName = "Enemy",
        order = 2)]
    public class EnemyTemplate : CharacterTemplate
    {
        public int HpLow;
        public int HpHigh;

    }
}
