using System;
using System.Collections.Generic;
using System.Linq;
using TestGame;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class CharacterFactory : ICharacterFactory
{
    private readonly PlayerConfiguration playerConfig;

    private readonly NonPlayableCharacterConfiguration enemyConfig;
    
    
    private readonly Transform playerPivot;

    private readonly List<Transform> enemyPivots;

    private readonly IAssetProviderForCharacter provider;
    private readonly Canvas canvas;

    internal CharacterFactory(IAssetProviderForCharacter levelInitializer, Transform playerPivot, List<Transform> enemyPivots, Canvas canvas)
    {
        this.playerPivot = playerPivot;
        this.canvas = canvas;
        this.enemyPivots = enemyPivots;
        provider = levelInitializer;
        playerConfig = Resources.Load<PlayerConfiguration>("Player");
        enemyConfig = Resources.Load<NonPlayableCharacterConfiguration>("Enemy");
    }

    internal Tuple<CharacterObject, List<CharacterObject>> CreateCharacters(PlayerTemplate playerTemplate, List<EnemyTemplate> enemyTemplate)
    {
        List<CharacterObject> enemies = new();
        foreach (var enemy in enemyTemplate)
        {
            enemies.Add(CreateEnemy(enemy));
        }

        var player = CreatePlayer(playerTemplate);
        return new Tuple<CharacterObject, List<CharacterObject>>(player, enemies);
    }

    public CharacterObject CreatePlayer(PlayerTemplate playerTemplate)
    {
        var player = provider.Instantiate(playerTemplate.obj, playerPivot);
        Assert.IsNotNull(player);



        var hp = playerConfig.Hp;
        playerConfig.Mana.Value =  playerTemplate.Mana;
        var shield = playerConfig.Shield;
        hp.Value = playerTemplate.Hp;
        shield.Value = 0;

        player.Configure(playerConfig);

        CreateHpWidget(playerConfig.HpWidget, player.gameObject, hp, shield);

        return player;
    }


    public CharacterObject CreateEnemy(EnemyTemplate enemyTemplate)
    {
        Transform enemyPivot = enemyPivots[0];
        foreach (var pivot in enemyPivots.Where(pivot => pivot.childCount == 0))
        {
            enemyPivot = pivot;
        }
        var enemy = provider.Instantiate(enemyTemplate.obj, enemyPivot);
        Assert.IsNotNull(enemy);

        var initialHp = Random.Range(enemyTemplate.HpLow, enemyTemplate.HpHigh + 1);
        var hp = enemyConfig.Hp;
        var shield = enemyConfig.Shield;
        hp.Value = initialHp;
        shield.Value = 0;

        CreateHpWidget(enemyConfig.HpWidget, enemy.gameObject, hp, shield);

        enemy.Character = new RuntimeCharacter
        {
            Hp = hp,
            Shield = shield,
        };
        return enemy;
    }
    
    
    private void CreateHpWidget(GameObject prefab, GameObject character, IntVariable hp, IntVariable shield)
    {
        var hpWidget = provider.Instantiate(prefab, canvas.transform, false);
        var pivot = character.transform;
        var canvasPos = Camera.main.WorldToViewportPoint(pivot.position + new Vector3(0.0f, -0.5f, 0.0f));
        hpWidget.GetComponent<RectTransform>().anchorMin = canvasPos;
        hpWidget.GetComponent<RectTransform>().anchorMax = canvasPos;
        hpWidget.GetComponent<HpWidget>().Initialize(hp, shield);
    }


    public GameObject CreateHud()
    {
        return null;
    }

}