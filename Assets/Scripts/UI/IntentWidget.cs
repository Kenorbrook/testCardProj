using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestGame
{
    //Don't use this in game, because the effect selection system is used after the player's turn
    public class IntentWidget : MonoBehaviour
    {
        [SerializeField]
        private Image intentImage;
        [SerializeField]
        private TextMeshProUGUI amountText;

        private const float InitialDelay = 1.25f;
        private const float FadeInDuration = 0.8f;
        private const float FadeOutDuration = 0.5f;

        private void Start()
        {
            var transparentColor = intentImage.color;
            transparentColor.a = 0.0f;
            intentImage.color = transparentColor;
            amountText.color = transparentColor;
        }

        internal void OnEnemyTurnBegan()
        {
            var seq = DOTween.Sequence();
            seq.AppendInterval(InitialDelay);
            seq.Append(intentImage.DOFade(0.0f, FadeOutDuration));

            seq = DOTween.Sequence();
            seq.AppendInterval(InitialDelay);
            seq.Append(amountText.DOFade(0.0f, FadeOutDuration));
        }

        internal void OnIntentChanged(Sprite sprite, int value)
        {
            intentImage.sprite = sprite;
            intentImage.SetNativeSize();
            amountText.text = value.ToString();

            intentImage.DOFade(1.0f, FadeInDuration);
            amountText.DOFade(1.0f, FadeInDuration);
        }
    }
}
