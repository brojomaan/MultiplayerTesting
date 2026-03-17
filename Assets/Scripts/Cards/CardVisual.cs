using System.Numerics;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace Cards
{
    public class CardVisual : MonoBehaviour
    {
        [Header("Card Transforms")] 
        [SerializeField] private RectTransform rootRect;
        [SerializeField] private Transform shakeTransform;
        [SerializeField] private Transform tiltTransform;
        [SerializeField] private Transform cardContainer;
        
        [Header("Card Visuals")]
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameText;

        [Header("Numbers")] 
        [SerializeField] private float tiltSpeed = 1f;
        [SerializeField] private float tiltAmount = 10f;
        [SerializeField] private float playerTiltAmount = 10f;

        [Header("Colours")] 
        public Color shadowColour;
        public Color cardColour;
        public Color highlightColour;
        public Color selectedColour;

        private Tween currentTween;
        private Tween colourTween;
        private Sequence currentSequence;
        private Vector3 lastMousePos;
        private float tiltOffset;

        private void Start()
        {
            tiltOffset = Random.Range(0f, 100f);
        }
        public void Wrap(CardData data)
        {
            nameText.text = data.displayName;
            icon.sprite = data.icon;
        }

        public void Shake()
        {
            currentTween?.Kill();

            currentTween = shakeTransform.DOShakeRotation(1f, 5f).SetEase(Ease.InOutQuad);
        }

        public void ResetShake()
        {
            currentTween?.Kill();
            currentTween = shakeTransform.DOLocalRotate(Vector3.zero, 0.2f).SetEase(Ease.InOutQuad);
        }
    }
}