using System.Numerics;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;

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
        [SerializeField] private Image card;
        [SerializeField] private Image shadow;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameText;

        [Header("Numbers")] 
        [SerializeField] private float tiltSpeed = 1f;
        [SerializeField] private float tiltAmount = 10f;
        [SerializeField] private float playerTiltAmount = 1f;

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

            currentTween = shakeTransform.DOShakeRotation(0.25f, 10f).SetEase(Ease.InOutQuad);
        }

        public void Scale(float amount, float duration)
        {
            currentTween?.Kill();

            currentTween = cardContainer.DOScale(amount, duration).SetEase(Ease.InOutQuad);
        }

        public void ResetShake()
        {
            currentTween?.Kill();
            currentTween = shakeTransform.DOLocalRotate(Vector3.zero, 0.25f).SetEase(Ease.InOutQuad);
        }

        public void IdleTilt()
        {
            float tiltX = Mathf.Sin(Time.time * tiltSpeed) * tiltAmount;
            float tiltY = Mathf.Cos(Time.time * tiltSpeed) * tiltAmount;
        
            tiltTransform.transform.rotation = Quaternion.Euler(tiltX, tiltY, 0f);
        }

        public void PlayerTilt()
        {
            if (Mouse.current == null) return;

            Vector2 mouse = Mouse.current.position.ReadValue();

            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, tiltTransform.position);

            Vector2 delta = mouse - screenPos;

            float tiltX = delta.y * 0.02f * playerTiltAmount;
            float tiltY = -delta.x * 0.02f * playerTiltAmount;

            Quaternion target = Quaternion.Euler(tiltX, tiltY, 0f);

            tiltTransform.rotation = Quaternion.Lerp(tiltTransform.rotation, target, Time.deltaTime * 10);
        }

        public void TiltReset()
        {
            currentTween?.Kill();
            currentTween = tiltTransform.DOLocalRotate(Vector3.zero, 0.2f).SetEase(Ease.InOutQuad);
        }

        public void SwapColour(Color newColour)
        {
            colourTween?.Kill();
            colourTween = card.DOColor(newColour, 0.2f).SetEase(Ease.InOutQuad);
        }

        public void Selected()
        {
            currentTween?.Kill();
            currentSequence?.Kill();
            TiltReset();
            
            Sequence selected = DOTween.Sequence();

            currentSequence = selected;

            selected.Append(cardContainer.DOLocalMoveY(50f, 0.2f)).SetEase(Ease.InOutQuad);
            selected.Join(cardContainer.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.2f).SetEase(Ease.InOutQuad));
            selected.Join(shadow.transform.DOLocalMove(new Vector3(0f, -35f, 0f), 0.2f).SetEase(Ease.InOutQuad));
            selected.Join(shadow.DOColor(new Vector4(shadowColour.r, shadowColour.g, shadowColour.b, 0.3f), 
                0.3f).SetEase(Ease.InOutQuad));
        }
        
        public void Deselected()
        {
            currentTween?.Kill();
            currentSequence?.Kill();
            TiltReset();
            
            Sequence deSelected = DOTween.Sequence();
            currentSequence = deSelected;
    
            deSelected.Append(cardContainer.DOLocalMoveY(0f, 0.2f)).SetEase(Ease.InOutQuad);
            deSelected.Join(cardContainer.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetEase(Ease.InOutQuad));
            deSelected.Join(shadow.transform.DOLocalMove(new Vector3(0f, -5f, 0f), 0.2f).SetEase(Ease.InOutQuad));
            deSelected.Join(shadow.DOColor(new Vector4(shadowColour.r, shadowColour.g, shadowColour.b, 0.8f), 
                0.3f).SetEase(Ease.InOutQuad));
        }

        public void OnBeginDrag()
        {
            lastMousePos = Mouse.current.position.ReadValue();
        }

        public void DragRotate()
        {
            Vector3 mouse = Mouse.current.position.ReadValue();
            Vector3 mouseDelta = mouse - lastMousePos;

            float rotateZ = -mouseDelta.x * 10f;

            Quaternion targetRotation = Quaternion.Euler(0f, 0f, rotateZ);

            tiltTransform.transform.rotation = Quaternion.Lerp(rootRect.transform.localRotation, targetRotation, Time.deltaTime * 10);
    
            lastMousePos = Mouse.current.position.ReadValue();
        }

        public void OnEndDrag()
        {
            currentTween?.Kill();
            currentTween = tiltTransform.transform.DORotate(Vector3.zero, 0.25f).SetEase(Ease.InOutQuad);
            
        }

    }
}