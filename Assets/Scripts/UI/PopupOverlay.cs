using DG.Tweening;
using UnityEngine;

namespace TestGame
{
    public class PopupOverlay : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        private const float FadeInTime = 0.4f;

        internal void Show()
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOFade(1.0f, FadeInTime);
        }
    }
}
