using UnityEngine;

namespace TestGame
{
    public class ManaResetSystem : BaseSystem
    {
        [SerializeField]
        private PlayerConfiguration playerConfig;

        private int playerMana;

        internal void Initialize(int mana)
        {
            playerMana = mana;
        }
        internal void OnPlayerTurnBegan()
        {
            playerConfig.Mana.SetValue(playerMana);
        }
    }
}
