using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public class DeckVisual : MonoBehaviour
    {
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");

        [Header("Transforms")]
        [SerializeField] private RectTransform rootRect;
        [SerializeField] private Transform deck;
        
        [Header("Visuals")]
        [SerializeField] private Image background;
        [SerializeField] private Image border;

        private Tween currentTweenDeck;
        private Tween currentTweenBorder;

        public void Initialize()
        {
            Material tempMaterial = Instantiate(background.material);
            background.material = tempMaterial;
        }
        public void ShakeDeck()
        {
            currentTweenDeck?.Kill();

            deck.DOShakePosition(0.3f, 10f).SetEase(Ease.InOutQuad);
        }

        public void BackgroundAlpha(float alphaMin, float alphaMax, float duration)
        {
            StartCoroutine(BackgroundAlphaRoutine(alphaMin, alphaMax, duration));
        }

        public void BorderScale(float value, float duration)
        {
            currentTweenBorder?.Kill();
            
            currentTweenBorder = border.rectTransform.DOScale(value, duration).SetEase(Ease.InOutQuad);
        }

        private IEnumerator BackgroundAlphaRoutine(float alphaMin, float alphaMax, float duration)
        {
            float t = 0;
            float opacity = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;
                opacity = Mathf.Lerp(alphaMin, alphaMax, t / duration);
                background.material.SetFloat(Alpha, opacity);
                yield return null;
            }

            background.material.SetFloat(Alpha, opacity);

        }

        public void ResetPosition()
        {
            currentTweenDeck?.Kill();

            deck.DOLocalMove(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad);
        }
    }
}
