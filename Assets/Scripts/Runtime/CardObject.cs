using DG.Tweening;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace TestGame
{
    public class CardObject : MonoBehaviour
    {
        internal RuntimeCard RuntimeCard;

        internal CardTemplate Template;

        [SerializeField]
        internal SortingGroup sortingGroup;

        [SerializeField]
        private TextMeshPro costText;

        [SerializeField]
        private TextMeshPro nameText;

        [SerializeField]
        private TextMeshPro descriptionText;

        [SerializeField]
        private SpriteRenderer picture;

        [SerializeField]
        private SpriteRenderer glow;

        [SerializeField]
        private Color inHandColor;

        [SerializeField]
        private Color aboutToBePlayedColor;


        private Vector3 cachedPos;
        private Quaternion cachedRot;
        private int cachedSortingOrder;
        private int highlightedSortingOrder;

        private bool interactable;

        private HandPresentationSystem handPresentationSystem;
        private CardWithoutTargetSelectionSystem cardWithoutArrowSelectionSystem;

        private bool beingHighlighted;
        private bool beingUnhighlighted;

        internal enum CardState
        {
            InHand,
            AboutToBePlayed
        }

        internal CardState State { get; private set; }

        private void Awake()
        {
            SetGlowEnabled(false);
        }
        
        private void OnEnable()
        {
            SetState(CardState.InHand);
        }

        private void OnMouseEnter()
        {
            if (interactable)
            {
                HighlightCard();
                handPresentationSystem.UnHighlightOtherCards(this);
            }
        }

        private void OnMouseExit()
        {
            if (interactable)
            {
                UnHighlightCard();
            }
        }

        internal void Init(RuntimeCard card, HandPresentationSystem presentationSystem, CardWithoutTargetSelectionSystem selectionSystem)
        {
            handPresentationSystem = presentationSystem;
            cardWithoutArrowSelectionSystem = selectionSystem;
            RuntimeCard = card;
            Template = card.Template;
            costText.text = Template.Cost.ToString();
            nameText.text = Template.Name;
            var builder = new StringBuilder();
            foreach (var effect in Template.Effects)
            {
                builder.AppendFormat("{0}. ", effect.GetName());
            }

            descriptionText.text = builder.ToString();
            picture.material = Template.Material;
            picture.sprite = Template.Picture;
        }

        internal void SetGlowEnabled(int playerMana)
            => SetGlowEnabled(playerMana >= Template.Cost);


        private void SetGlowEnabled(bool glowEnabled)
            => glow.enabled = glowEnabled;


        internal void OnManaChanged(int mana)
        {
            SetGlowEnabled(Template.Cost <= mana);
        }

        internal void SetState(CardState state)
        {
            State = state;
            glow.color = State switch
            {
                CardState.InHand => inHandColor,
                CardState.AboutToBePlayed => aboutToBePlayedColor,
                _ => glow.color
            };
        }

        internal void SetInteractable(bool value)
        {
            interactable = value;
        }

        internal void CacheTransform(Vector3 position, Quaternion rotation)
        {
            cachedPos = position;
            cachedRot = rotation;
            cachedSortingOrder = sortingGroup.sortingOrder;
            highlightedSortingOrder = cachedSortingOrder + 10;
        }


        internal void UnHighlightCard()
        {
            if (CardSelectionSystem.HasSelectedCard() || beingUnhighlighted)
            {
                return;
            }

            beingUnhighlighted = true;

            sortingGroup.sortingOrder = cachedSortingOrder;
            transform.DOMove(cachedPos, 0.02f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => beingUnhighlighted = false);
        }

        internal void ResolveEffects(CharacterObject instigator, CharacterObject target)
        {
            foreach (var entityEffect in Template.Effects.OfType<TargetEffect>())
            {
                entityEffect.Resolve(instigator.Character, target);
            }
        }

        internal void Reset()
        {
            transform.DOMove(cachedPos, 0.2f);
            transform.DORotateQuaternion(cachedRot, 0.2f);
            sortingGroup.sortingOrder = cachedSortingOrder;
        }

        private void HighlightCard()
        {
            if (CardSelectionSystem.HasSelectedCard() || beingHighlighted)
            {
                return;
            }

            beingHighlighted = true;

            sortingGroup.sortingOrder = highlightedSortingOrder;
            transform.DOMove(cachedPos + new Vector3(0.0f, 1.0f, 0.0f), 0.05f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => beingHighlighted = false);
        }
    }
}