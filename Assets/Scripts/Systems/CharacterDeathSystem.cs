namespace TestGame
{
    public class CharacterDeathSystem : BaseSystem
    {
        public void OnPlayerHpChanged(int hp)
        {
            if (hp <= 0)
            {
                EndGame(true);
            }
        }

        public void OnEnemyHpChanged(int hp)
        {
            if (hp <= 0)
            {
                EnemyDied();
            }
        }

        private void EnemyDied()
        {
            var deathEnemy = Enemy.Find(enemy => enemy.Character.Hp.Value <= 0);
            Enemy.Remove(deathEnemy);
            Destroy(deathEnemy.gameObject);
            if(Enemy.Count>0) return;
            EndGame(false);
        }
        //TODO inits popup via initializer :( 
        private void EndGame(bool playerDied)
        {
            var popupOverlay = FindObjectOfType<PopupOverlay>();
            var endBattlePopup = FindObjectOfType<EndBattlePopup>();
            if (popupOverlay == null || endBattlePopup == null) return;
            
            popupOverlay.Show();
            endBattlePopup.Show();

            if (playerDied)
            {
                endBattlePopup.SetDefeatText();
            }
            else
            {
                endBattlePopup.SetVictoryText();
            }

            var turnManagementSystem = FindObjectOfType<TurnManagementSystem>();
            turnManagementSystem.SetEndOfGame(true);
        }
    }
}
