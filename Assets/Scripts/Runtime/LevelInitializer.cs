using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TestGame
{
    public class LevelInitializer : MonoBehaviour,IAssetProviderForCharacter
    {
        [SerializeField]
        private Transform _playerPivot;

        [SerializeField]
        private Canvas _mainCanvas;
        [SerializeField]
        private List<Transform> _enemyPivot;
        [SerializeField]
        private DeckDrawingSystem deckDrawingSystem;

        [SerializeField]
        private HandPresentationSystem handPresentationSystem;

        [SerializeField]
        private TurnManagementSystem turnManagementSystem;

        [SerializeField]
        private EnemyBrainSystem enemyBrainSystem;

        [SerializeField]
        private ManaResetSystem _manaResetSystem;
        [SerializeField]
        private ManaWidget manaWidget;

        [SerializeField]
        private DeckWidget deckWidget;

        [SerializeField]
        private LevelsData _levels;

        [SerializeField]
        private DiscardPileWidget discardPileWidget;

        [SerializeField]
        private CardWithTargetSelectionSystem cardWithArrowSelectionSystem;

        [SerializeField]
        private CardWithoutTargetSelectionSystem cardWithoutArrowSelectionSystem;

        [SerializeField]
        private CardPool cardPool;

        private CardLibrary playerDeck;


        private CharacterFactory factory;

        private void Awake()
        {
            factory = new CharacterFactory(this, _playerPivot, _enemyPivot, _mainCanvas);
           
           var characters= CreateFakeLevel();

            InitializeGame(characters.Item1, characters.Item2);
            cardPool.Initialize();
        }

        private Tuple<CharacterObject, List<CharacterObject>> CreateFakeLevel()
        {
            var tuple =  factory.CreateCharacters(_levels._player, _levels.GetLevel(0)._enemies);
            
            playerDeck = _levels._player.StartingDeck;
            manaWidget.Initialize(tuple.Item1.Character.Mana);
            return tuple;
        }

        private void CreateLevel(int level)
        {
            foreach (var enemyTemplate in _levels.GetLevel(level)._enemies)
            {
                factory.CreateEnemy(enemyTemplate);
            }
        }


        private void InitializeGame(CharacterObject player, List<CharacterObject> enemies)
        {
            deckDrawingSystem.Initialize(deckWidget, discardPileWidget);
            deckDrawingSystem.LoadDeck(playerDeck);
            deckDrawingSystem.ShuffleDeck();
            cardWithArrowSelectionSystem.Initialize(player, enemies);
            cardWithoutArrowSelectionSystem.Initialize(player, enemies);
            handPresentationSystem.Initialize(cardPool, deckWidget, discardPileWidget, cardWithoutArrowSelectionSystem);
            _manaResetSystem.Initialize(player.Character.Mana.Value);
            enemyBrainSystem.Initialize(player, enemies);

            turnManagementSystem.BeginGame();
        }

        public CharacterObject Instantiate(CharacterObject playerTemplate, Transform at)
        {
            return Object.Instantiate(playerTemplate, at);
        }

        public GameObject Instantiate(GameObject obj, Transform at, bool instantiateInWorldSpace)
        {
            return Object.Instantiate(obj, at, instantiateInWorldSpace);
        }

        public CharacterObject Instantiate(CharacterObject playerTemplate)
        {
            return Object.Instantiate(playerTemplate);
        }
    }
}