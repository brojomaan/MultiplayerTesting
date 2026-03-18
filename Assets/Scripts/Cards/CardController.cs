using Database;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

namespace Cards
{
    public class CardController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [Header("References")] 
        [SerializeField] private RectTransform rootRect;
        [SerializeField] private CardData data;
        [SerializeField] private CardVisual visual;
        [SerializeField] private CardDockController dock;
        
        [Header("Logic")]
        [SerializeField] private int indexInHand;
        [SerializeField] private NetworkPlayer owner;
        [SerializeField] private Image iCollider;

        [Header("States")] 
        [SerializeField] private bool isSelected;
        [SerializeField] private bool isHovered;
        [SerializeField] private bool isAnimating;
        [SerializeField] private bool isDragging;

        private Vector3 velocity;
        
        public void Initialize(int cardId, CardVisual cardVisual, NetworkPlayer player, int index, CardDockController dockController)
        {
            data = CardDatabase.Instance.GetCard(cardId);

            owner = player;
            indexInHand = index;
            dock = dockController;
            
            visual = cardVisual;
            visual.Wrap(data);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isDragging) return;
            isHovered = true;
            visual.Shake();
            visual.SwapColour(visual.highlightColour);
            visual.Scale(1.1f, 0.2f);

        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (isDragging) return;
            isHovered = false;
            visual.ResetShake();
            visual.SwapColour(visual.cardColour);
            visual.Scale(1f, 0.2f);
        }
    
        public void OnPointerClick(PointerEventData eventData)
        {
            if (isDragging) return;
            if (!isSelected)
            {
                isSelected = true;
                visual.SwapColour(visual.selectedColour);
                visual.Scale(1.5f, 0.2f);
            }
            else
            {
                isSelected = false;
                visual.SwapColour(visual.highlightColour);
                visual.Scale(1.1f, 0.2f);
            }

            /*owner.SubmitCardServerRpc(indexInHand);*/
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isSelected) return;

            dock.SetSelectedCard(this);
            isDragging = true;
            
            transform.SetAsLastSibling();
            iCollider.raycastTarget = false;
            

            visual.Selected();
            visual.OnBeginDrag();
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rootRect.parent as RectTransform,
                eventData.position,
                Camera.main,
                out Vector2 localPoint
            );

            Vector3 targetPosition = localPoint;

            rootRect.localPosition = Vector3.SmoothDamp(
                rootRect.localPosition,
                targetPosition,
                ref velocity,
                0.04f
            );
            visual.DragRotate();
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            visual.Deselected();
            visual.OnEndDrag();
            dock.PlaceDraggedCard();
            iCollider.raycastTarget = true;
            
            isDragging = false;
        }
        
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void SetIndex(int index)
        {
            indexInHand = index;
        }

        private void Update()
        {
            if (isSelected || isDragging) return;
            if (!isHovered)
            {
                visual.IdleTilt();
            }
            else
            {
                visual.PlayerTilt();
            }
        }
    }
}
