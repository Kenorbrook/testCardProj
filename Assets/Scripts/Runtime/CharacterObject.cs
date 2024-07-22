using UnityEngine;

namespace TestGame
{
    public class CharacterObject : MonoBehaviour
    {
        public RuntimeCharacter Character;

        internal void Configure(PlayerConfiguration playerConfig)
        {
            Character = new RuntimeCharacter
            {
                Hp = playerConfig.Hp,
                MaxHp = playerConfig.Hp.Value,
                Shield = playerConfig.Shield,
                Mana = playerConfig.Mana,
            };
        }
    }
}