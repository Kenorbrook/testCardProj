using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace TestGame
{
    public class CardWithoutTargetSelectionSystem : CardSelectionSystem
    {
        private void Update()
        {
            if (TurnManagementSystem.IsEndOfGame())
                return;

            if (HandPresentationSystem.IsAnimating())
                return;

            if (IsCardAboutToBePlayed)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                DetectCardSelection();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                DetectCardUnselection();
            }

            if (SelectedCard != null)
            {
                UpdateSelectedCard();
            }
        }

        private void DetectCardSelection()
        {
            if (SelectedCard != null)
                return;
            
            var mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hitInfo = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity, CardLayer);
            
            if (hitInfo.collider == null) return;
            
            
            var card = hitInfo.collider.GetComponent<CardObject>();
            var cardTemplate = card.Template;
            if (CardUtils.CardCanBePlayed(cardTemplate, PlayerMana) &&
                !CardUtils.CardHasTargetableEffect(cardTemplate))
            {
                SelectedCard = card;
                IsCardSelected = true;
                var transform1 = SelectedCard.transform;
                OriginalCardPos = transform1.position;
                OriginalCardRot = transform1.rotation;
                OriginalCardSortingOrder = SelectedCard.sortingGroup.sortingOrder;
            }
        }

        private void UpdateSelectedCard()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (SelectedCard.State == CardObject.CardState.AboutToBePlayed)
                {
                    IsCardAboutToBePlayed = true;
                    
                    var seq = DOTween.Sequence();
                    seq.AppendInterval(CardAnimationTime + 0.1f);
                    seq.AppendCallback(() =>
                    {
                        PlaySelectedCard(Player);
                        SelectedCard = null;
                        IsCardSelected = false;
                        IsCardAboutToBePlayed = false;
                    });
                    SelectedCard.transform.DORotate(Vector3.zero, CardAnimationTime);
                }
                else
                {
                    SelectedCard.SetState(CardObject.CardState.InHand);
                    SelectedCard.Reset();
                    SelectedCard = null;
                    IsCardSelected = false;
                }
            }

            if (SelectedCard == null) return;
            
            
            var mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            SelectedCard.transform.position = mousePos;

            SelectedCard.SetState(mousePos.y > OriginalCardPos.y + CardAboutToBePlayedOffsetY
                ? CardObject.CardState.AboutToBePlayed
                : CardObject.CardState.InHand);
        }
        
        private void DetectCardUnselection()
        {
            if (SelectedCard == null) return;
            
            var seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                SelectedCard.SetState(CardObject.CardState.InHand);
                SelectedCard.transform
                    .DOMove(OriginalCardPos, CardSelectionCanceledAnimationTime)
                    .SetEase(CardAnimationEase);
                SelectedCard.transform.DORotate(OriginalCardRot.eulerAngles, CardSelectionCanceledAnimationTime);
            });
            seq.OnComplete(() =>
            {
                SelectedCard.sortingGroup.sortingOrder = OriginalCardSortingOrder;
                SelectedCard = null;
                IsCardSelected = false;
            });
        }

       
    }
}
