using DG.Tweening;
using UnityEngine;

namespace TestGame
{
    public abstract class CardSelectionSystem : BaseSystem
    {
        public IntVariable PlayerMana;

        public TurnManagementSystem TurnManagementSystem;
        public DeckDrawingSystem DeckDrawingSystem;
        public HandPresentationSystem HandPresentationSystem;

        protected int OriginalCardSortingOrder;
        protected Camera MainCamera;
        protected LayerMask CardLayer;
        protected LayerMask EnemyLayer;
        
        protected Quaternion OriginalCardRot;

        protected bool IsCardAboutToBePlayed;
        protected Vector3 OriginalCardPos;
        protected CardObject SelectedCard;

        protected static bool IsCardSelected;
        
        protected const float CardSelectionCanceledAnimationTime = 0.2f;
        protected const Ease CardAnimationEase = Ease.OutBack;
        
        protected const float CardAboutToBePlayedOffsetY = 1.5f;
        protected const float CardAnimationTime = 0.1f;

        protected virtual void Start()
        {
            CardLayer = 1 << LayerMask.NameToLayer("Card");
            EnemyLayer = 1 << LayerMask.NameToLayer("Enemy");
            MainCamera = Camera.main;
        }

        protected void PlaySelectedCard(CharacterObject instigator, CharacterObject target = null)
        {
            
            SelectedCard.SetInteractable(false);
            PlayerMana.SetValue(PlayerMana.Value - SelectedCard.Template.Cost);
            SelectedCard.ResolveEffects(instigator, target);

            HandPresentationSystem.RearrangeHand(SelectedCard);
            HandPresentationSystem.RemoveCardFromHand(SelectedCard);
            HandPresentationSystem.MoveCardToDiscardPile(SelectedCard);

            DeckDrawingSystem.MoveCardToDiscardPile(SelectedCard.RuntimeCard);

        } 
        
       

        internal static bool HasSelectedCard()
        {
            return IsCardSelected;
        }
    }
}
