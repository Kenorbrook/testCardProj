using UnityEngine;

namespace TestGame
{
    public class TurnManagementSystem : MonoBehaviour
    {
        public GameEvent PlayerTurnBegan;
        public GameEvent PlayerTurnEnded;
        public GameEvent EnemyTurnBegan;
        public GameEvent EnemyTurnEnded;

        private bool isEnemyTurn;
        private float accTime;

        private bool isEndOfGame;

        private const float EnemyTurnDuration = 3.0f;

        protected void Update()
        {
            if (!isEnemyTurn) return;
            
            
            accTime += Time.deltaTime;
            if (accTime >= EnemyTurnDuration)
            {
                accTime = 0.0f;
                EndEnemyTurn();
                BeginPlayerTurn();
            }
        }

        internal void BeginGame()
        {
            BeginPlayerTurn();
        }


        internal void EndPlayerTurn()
        {
            PlayerTurnEnded.Raise();
            BeginEnemyTurn();
        }


        internal void SetEndOfGame(bool value)
        {
            isEndOfGame = value;
        }

        internal bool IsEndOfGame()
        {
            return isEndOfGame;
        }
        
        private void BeginEnemyTurn()
        {
            EnemyTurnBegan.Raise();
            isEnemyTurn = true;
        }
        
        private void EndEnemyTurn()
        {
            EnemyTurnEnded.Raise();
            isEnemyTurn = false;
        }
        private void BeginPlayerTurn()
        {
            PlayerTurnBegan.Raise();
        }
    }
}
