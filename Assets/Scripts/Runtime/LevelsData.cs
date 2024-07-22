using System.Collections.Generic;
using TestGame;
using UnityEngine;

public class LevelsData : MonoBehaviour
{
    [SerializeField]
    private List<LevelData> _data;

    [SerializeField]
    internal PlayerTemplate _player;
    internal LevelData GetLevel(int num) => _data[num];
}