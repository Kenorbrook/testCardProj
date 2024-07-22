using UnityEngine;

namespace TestGame
{
    [CreateAssetMenu(
        menuName = "Configuration/Non-player character",
        fileName = "NPCConfiguration",
        order = 1)]
    public class NonPlayableCharacterConfiguration : ScriptableObject
    {
        public IntVariable Hp;
        public IntVariable Shield;


        public GameObject HpWidget;
    }
}