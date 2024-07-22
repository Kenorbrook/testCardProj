using TMPro;
using UnityEngine;

namespace TestGame
{
    public class ManaWidget : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        private int maxValue;

        internal void Initialize(IntVariable mana)
        {
            maxValue = mana.Value;
            SetValue(mana.Value);
        }
        internal void OnManaChanged(int value)
        {
            SetValue(value);
        }
        private void SetValue(int value)
        {
            text.text = $"{value}/{maxValue}";
        }

      
    }
}
