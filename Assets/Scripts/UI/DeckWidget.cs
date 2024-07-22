using TMPro;
using UnityEngine;

namespace TestGame
{
    public class DeckWidget : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text textLabel;
        private int deckSize;

        internal void SetAmount(int amount)
        {
            deckSize = amount;
            textLabel.text = amount.ToString();
        }

        internal void RemoveCard()
        {
            SetAmount(deckSize - 1);
        }
    }
}
