using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestGame
{
    public class DeckDrawingSystem : MonoBehaviour
    {
        public HandPresentationSystem HandPresentationSystem;

        private List<RuntimeCard> deck;
        private List<RuntimeCard> discardPile;
        private List<RuntimeCard> hand;

        private DeckWidget deckWidget;
        private DiscardPileWidget discardPileWidget;

        
        private const int InitialDeckCapacity = 300;
        private const int InitialDiscardPileCapacity = 30;
        private const int InitialHandCapacity = 300;

        internal void Initialize(DeckWidget deck, DiscardPileWidget discardPile)
        {
            deckWidget = deck;
            discardPileWidget = discardPile;
        }
        

        internal void LoadDeck(CardLibrary playerDeck)
        {
            deck = new List<RuntimeCard>(InitialDeckCapacity);
            discardPile = new List<RuntimeCard>(InitialDiscardPileCapacity);
            hand = new List<RuntimeCard>(InitialHandCapacity);
            var library = playerDeck;
            foreach (var entry in library.Entries.Where(entry => entry.Card != null))
            {
                for (var i = 0; i < entry.NumCopies; i++)
                {
                    var card = new RuntimeCard 
                    {
                        Template = entry.Card
                    };
                    deck.Add(card);
                }
            }

            deckWidget.SetAmount(deck.Count);
            discardPileWidget.SetAmount(0);
        }

        internal void ShuffleDeck()
        {
            deck.Shuffle();
        }

        internal void DrawCardsFromDeck(int amount)
        {
            var deckSize = deck.Count;
            if (deckSize >= amount)
            {
                List<RuntimeCard> drawnCards = new(amount);
                for (var i = 0; i < amount; i++)
                {
                    var card = deck[0];
                    deck.RemoveAt(0);
                    hand.Add(card);
                    drawnCards.Add(card);
                }

                HandPresentationSystem.CreateCardsInHand(drawnCards, deckSize);
            }
            else
            {
                foreach (var runtimeCard in discardPile)
                    deck.Add(runtimeCard);

                discardPile.Clear();

                HandPresentationSystem.UpdateDiscardPileSize(discardPile.Count);

                
                if (amount > deck.Count + discardPile.Count)
                {
                    amount = deck.Count + discardPile.Count;
                }
                DrawCardsFromDeck(amount);
            }
        }

        internal void MoveCardToDiscardPile(RuntimeCard card)
        {
            var idx = hand.IndexOf(card);
            hand.RemoveAt(idx);
            discardPile.Add(card);
        }

        internal void MoveCardsToDiscardPile()
        {
            foreach (var card in hand)
                discardPile.Add(card);

            hand.Clear();
        }
    }
}
