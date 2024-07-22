using TestGame;
using UnityEngine;

public interface ICharacterFactory
{
    CharacterObject CreatePlayer(PlayerTemplate playerTemplate);
    CharacterObject CreateEnemy(EnemyTemplate enemyTemplate);
    GameObject CreateHud();
}