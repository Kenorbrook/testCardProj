using System.Collections.Generic;
using TestGame;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Level", fileName = "LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    [SerializeField]
    internal List<EnemyTemplate> _enemies;
}