using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace TestGame
{

    public class HandPresentationSystem : MonoBehaviour
    {
        public IntVariable playerMana;

        private CardPool cardPool;
        private DeckWidget deckWidget;
        private DiscardPileWidget discardPileWidget;

        private readonly List<CardObject> handCards = new(PositionsCapacity);

        private bool isAnimating;

        private readonly List<Vector3> positions = new(PositionsCapacity);
        private readonly List<Quaternion> rotations = new(RotationsCapacity);
        private readonly List<int> sortingOrders = new(SortingOrdersCapacity);

        private const int PositionsCapacity = 30;
        private const int RotationsCapacity = 30;
        private const int SortingOrdersCapacity = 30;

        private const float CenterRadius = 16.0f;
        private readonly Vector3 centerPoint = new Vector3(0.0f, -20.5f, 0.0f);
        private readonly Vector3 originalCardScale = new Vector3(0.6f, 0.6f, 1.0f);

        private const float CardToDiscardPileAnimationTime = 0.3f;

        private CardWithoutTargetSelectionSystem cardWithoutTargetSelectionSystem;

        internal void Initialize(CardPool pool, DeckWidget deck, DiscardPileWidget discardPile, CardWithoutTargetSelectionSystem cardWithoutTargetSelectionSystem)
        {
            cardPool = pool;
            deckWidget = deck;
            discardPileWidget = discardPile;
            this.cardWithoutTargetSelectionSystem = cardWithoutTargetSelectionSystem;
        }


        internal void CreateCardsInHand(List<RuntimeCard> hand, int deckSize)
        {
            List<CardObject> drawnCards = new(hand.Count);
            foreach (var go in hand.Select(CreateCardGo))
            {
                handCards.Add(go);
                drawnCards.Add(go);
            }

            deckWidget.SetAmount(deckSize);

            AnimateCardsFromDeckToHand(drawnCards);
        }

        internal void UpdateDiscardPileSize(int size)
        {
            discardPileWidget.SetAmount(size);
        }

        internal bool IsAnimating()
        {
            return isAnimating;
        }

        
        
        internal void RearrangeHand(CardObject selectedCard)
        {
            handCards.Remove(selectedCard);

            ArrangeHandCards();

            for (var i = 0; i < handCards.Count; i++)
            {
                var card = handCards[i];
                const float time = 0.3f;
                card.transform.DOMove(positions[i], time);
                card.transform.DORotateQuaternion(rotations[i], time);
                card.sortingGroup.sortingOrder = sortingOrders[i];
                card.SetGlowEnabled(playerMana.Value);
                card.CacheTransform(positions[i], rotations[i]);
            }
        }

        internal void RemoveCardFromHand(CardObject go)
        {
            handCards.Remove(go);
        }

        internal void MoveCardToDiscardPile(CardObject go)
        {
            var seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                go.transform.DOMove(discardPileWidget.transform.position, CardToDiscardPileAnimationTime);
                go.transform.DOScale(Vector3.zero, CardToDiscardPileAnimationTime).OnComplete(() =>
                {
                    PooledCard pooledObject = go.GetComponent<PooledCard>();
                    cardPool.ReturnObject(pooledObject);
                });
            });
            seq.AppendCallback(() =>
            {
                discardPileWidget.AddCard();
                handCards.Remove(go);
            });
        }

        internal void MoveHandToDiscardPile()
        {
            foreach (var card in handCards)
                MoveCardToDiscardPile(card);
            handCards.Clear();
        }
        internal void UnHighlightOtherCards(CardObject x)
        {
            foreach (var card in handCards.Where(card => card != x))
            {
                card.UnHighlightCard();
            }
        }
        private CardObject CreateCardGo(RuntimeCard card)
        {
            CardObject go = cardPool.GetObject();
            go.Init(card, this, cardWithoutTargetSelectionSystem);
            go.SetGlowEnabled(playerMana.Value);
            var transform1 = go.transform;
            transform1.position = deckWidget.transform.position;
            transform1.localScale = Vector3.zero;

            return go;
        }

        private void AnimateCardsFromDeckToHand(ICollection<CardObject> drawnCards)
        {
            isAnimating = true;

            ArrangeHandCards();

            foreach (var card in handCards)
            {
                card.SetInteractable(false);
            }

            var interval = 0.0f;
            for (var i = 0; i < handCards.Count; i++)
            {
                var i1 = i;
                const float time = 0.5f;
                var card = handCards[i];
                if (drawnCards.Contains(card))
                {

                    var seq = DOTween.Sequence();
                    seq.AppendInterval(interval);
                    seq.AppendCallback(() =>
                    {
                        deckWidget.RemoveCard();
                        TweenerCore<Vector3, Vector3, VectorOptions> move = card.transform.DOMove(positions[i1], time).OnComplete(() =>
                        {
                            card.CacheTransform(positions[i1], rotations[i1]);
                            card.SetInteractable(true);
                        });
                        card.transform.DORotateQuaternion(rotations[i1], time);
                        card.transform.DOScale(originalCardScale, time);
                        if (i1 == handCards.Count - 1)
                        {
                            move.OnComplete(() =>
                            {
                                isAnimating = false;
                                card.CacheTransform(positions[i1], rotations[i1]);
                                card.SetInteractable(true);
                            });
                        }
                    });

                    card.sortingGroup.sortingOrder = sortingOrders[i];

                    interval += 0.2f;
                }
                else
                {
                    card.transform.DOMove(positions[i1], time).OnComplete(() =>
                    {
                        card.CacheTransform(positions[i1], rotations[i1]);
                        card.SetInteractable(true);
                    });
                    card.transform.DORotateQuaternion(rotations[i1], time);
                }
            }
        }

        private void ArrangeHandCards()
        {
            positions.Clear();
            rotations.Clear();
            sortingOrders.Clear();

            const float angle = 5.0f;
            var cardAngle = (handCards.Count - 1) * angle / 2;
            var z = 0.0f;
            for (var i = 0; i < handCards.Count; ++i)
            {
                // Rotate.
                var rotation = Quaternion.Euler(0, 0, cardAngle - i * angle);
                rotations.Add(rotation);

                // Move.
                z -= 0.1f;
                var position = CalculateCardPosition(cardAngle - i * angle);
                position.z = z;
                positions.Add(position);

                // Set sorting order.
                sortingOrders.Add(i);
            }
        }


        private Vector3 CalculateCardPosition(float angle)
        {
            return new Vector3(
                centerPoint.x - CenterRadius * Mathf.Sin(Mathf.Deg2Rad * angle),
                centerPoint.y + CenterRadius * Mathf.Cos(Mathf.Deg2Rad * angle),
                0.0f);
        }

       
    }
}