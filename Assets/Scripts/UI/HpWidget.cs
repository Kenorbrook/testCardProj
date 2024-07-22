using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestGame
{
    public class HpWidget : MonoBehaviour
    {
        [SerializeField]
        private Image hpBar;
        [SerializeField]
        private Image hpBarBackground;
        [SerializeField]
        private TextMeshProUGUI hpText;
        [SerializeField]
        private GameObject shieldGroup;
        [SerializeField]
        private TextMeshProUGUI shieldText;

        private int maxValue;

        internal void Initialize(IntVariable hp, IntVariable shield)
        {
            maxValue = hp.Value;
            SetHp(hp.Value);
            SetShield(shield.Value);
        }
        internal void OnHpChanged(int value)
        {
            SetHp(value);
        }

        internal void OnShieldChanged(int value)
        {
            SetShield(value);
        }
        private void SetHp(int value)
        {
            var newValue = value / (float)maxValue;
            hpBar.DOFillAmount(newValue, 0.2f)
                .SetEase(Ease.InSine);

            var seq = DOTween.Sequence();
            seq.AppendInterval(0.5f);
            seq.Append(hpBarBackground.DOFillAmount(newValue, 0.2f));
            seq.SetEase(Ease.InSine);

            hpText.text = $"{value.ToString()}/{maxValue.ToString()}";
        }

        private void SetShield(int value)
        {
            shieldText.text = $"{value.ToString()}";
            SetShieldActive(value > 0);
        }

        private void SetShieldActive(bool shieldActive)
        {
            shieldGroup.SetActive(shieldActive);
        }

       
    }
}
