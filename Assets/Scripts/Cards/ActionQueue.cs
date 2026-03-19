using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class ActionQueue : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        [SerializeField] private ActionQueueVisual actionQueueVisualPrefab;

        [SerializeField] private ActionQueueVisual visual;

        private void Start()
        {
            Initialize();
        }
        public void Initialize()
        {
            visual = Instantiate(actionQueueVisualPrefab, this.transform, false);
            visual.Initialize();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            visual.ShakeDeck();
            visual.BackgroundAlpha(0.6f, 1, 0.3f);
            visual.BorderScale(1.1f, 0.1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            visual.ResetPosition();
            visual.BackgroundAlpha(1f, 0.6f, 0.3f);
            visual.BorderScale(1f, 0.1f);
        }

        public void OnDrop(PointerEventData eventData)
        {
            CardController card = eventData.pointerDrag?.GetComponent<CardController>();

            if (card == null) return;

            QueueCard(card);
        }

        private void QueueCard(CardController card)
        {
            Debug.Log($"Queue card: {card.Data.displayName}");

            card.MarkDroppedOnZone();
        }
    }
}
