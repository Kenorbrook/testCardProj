using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace TestGame
{
    public class CardWithTargetSelectionSystem : CardSelectionSystem
    {
        private GameObject highlightedCard;


        private GameObject selectedEnemy;
        private bool isArrowCreated;
        private void Update()
        {
            if (TurnManagementSystem.IsEndOfGame())
                return;

            if (HandPresentationSystem.IsAnimating())
                return;

            
            
            if (Input.GetMouseButtonDown(0))
            {
                DetectCardSelection();
                DetectEnemySelection();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                DetectCardUnselection();
            }

            if (SelectedCard != null)
                UpdateSelectedCard();
        }

        private void DetectCardSelection()
        {
            var mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hitInfo = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity, CardLayer);
            if (hitInfo.collider != null)
            {
                var card = hitInfo.collider.GetComponent<CardObject>();
                var cardTemplate = card.Template;
                if (CardUtils.CardCanBePlayed(cardTemplate, PlayerMana) &&
                    CardUtils.CardHasTargetableEffect(cardTemplate))
                {
                    SelectedCard = card;
                    var transform1 = SelectedCard.transform;
                    OriginalCardPos = transform1.position;
                    OriginalCardRot = transform1.rotation;
                    IsCardSelected = true;
                    SelectedCard.sortingGroup.sortingOrder += 10;
                }
            }
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

        private void UpdateSelectedCard()
        {
            if (Input.GetMouseButtonUp(0))
            {
                DetectEnemySelection();
            }

            if (SelectedCard == null) return;
            
            
            var mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            SelectedCard.transform.position = mousePos;


            SelectedCard.SetState(mousePos.y > OriginalCardPos.y + CardAboutToBePlayedOffsetY
                ? CardObject.CardState.AboutToBePlayed
                : CardObject.CardState.InHand);
        }
        
        private void DetectEnemySelection()
        {
            if (SelectedCard == null) return;
            
            var mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hitInfo = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity, EnemyLayer);

            if (hitInfo.collider == null) return;
            
            selectedEnemy = hitInfo.collider.gameObject;
            PlaySelectedCard(Player, selectedEnemy.GetComponent<CharacterObject>());
            SelectedCard = null;
            IsCardSelected = false;
        }

       
        
    }
}
