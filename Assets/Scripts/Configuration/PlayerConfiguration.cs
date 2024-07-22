using UnityEngine;

namespace TestGame
{
    [CreateAssetMenu(
        menuName = "Configuration/Player character",
        fileName = "PlayerConfiguration",
        order = 0)]
    public class PlayerConfiguration : ScriptableObject
    {
        public IntVariable Hp;
        public IntVariable Shield;

        public IntVariable Mana;


        public GameObject HpWidget;
    }
}