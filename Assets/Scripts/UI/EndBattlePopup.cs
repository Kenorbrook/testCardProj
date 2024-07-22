using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TestGame
{
    public class EndBattlePopup : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI titleText;
        [SerializeField]
        private TextMeshProUGUI descriptionText;
        [SerializeField]
        private CanvasGroup canvasGroup;

        private const string VictoryText = "Victory!";
        private const string VictoryDescriptionText = "Congratulations! This section will include rewards in a future update.";
        private const string DefeatText = "Defeat!";
        private const string DefeatDescriptionText = "Better luck next time!";
        private const float FadeInTime = 0.4f;
        

        internal void Show()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOFade(1.0f, FadeInTime);
        }

        internal void SetVictoryText()
        {
            titleText.text = VictoryText;
            descriptionText.text = VictoryDescriptionText;
        }

        internal void SetDefeatText()
        {
            titleText.text = DefeatText;
            descriptionText.text = DefeatDescriptionText;
        }

        internal void OnContinueButtonPressed()
        {
            
        }
    }
}
