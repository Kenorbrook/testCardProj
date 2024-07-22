using UnityEngine;
using UnityEngine.UI;

namespace TestGame
{
    public class EndTurnButton : Button
    {

        private HandPresentationSystem handPresentationSystem;

        //Todo init hand System and turn system 
        protected override void Start()
        {
            base.Start();
            onClick.AddListener(OnButtonPressed);
            handPresentationSystem = FindObjectOfType<HandPresentationSystem>();
        }
        internal void OnPlayerTurnBegan()
        {
            interactable = true;
        }
        private void OnButtonPressed()
        {
            if (handPresentationSystem.IsAnimating())
            {
                return;
            }

            if (CardSelectionSystem.HasSelectedCard())
            {
                
                Debug.Log("Card was selected");
                return;
            }

            interactable = false;

            var system = FindObjectOfType<TurnManagementSystem>();
            system.EndPlayerTurn();
        }

      
    }
}
