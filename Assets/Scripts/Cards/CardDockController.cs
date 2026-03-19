using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class CardDockController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Prefabs")]
        [SerializeField] private CardDockVisual dockVisualPrefab;
        [SerializeField] private CardController cardControllerPrefab;
        [SerializeField] private CardVisual cardVisualPrefab;
        
        [Header("Transforms")]
        [SerializeField] private RectTransform rootRect;
        
        [Header("Visuals")]
        [SerializeField] private CardDockVisual visual;
        
        [Header("Logic")]
        [SerializeField] private List<CardController> cardControllers;
        [SerializeField] private float cardSpacing;
        [SerializeField] private CardController draggingCard;
        
        private int currentInsertIndex;
        
        
        private void Start()
        {
            Initialize();
        }
        public void Initialize()
        {
            /*visual = Instantiate(dockVisualPrefab, this.transform, false);
            visual.transform.position = this.transform.position;*/
            visual.Initialize();
        }
        
        public void ShowHand(int[] cardIds, NetworkPlayer owner)
        {
            Clear();
            
            float cardXOffset = (cardIds.Length - 1) * cardSpacing / 2;
            
            for (int i = 0; i < cardIds.Length; i++)
            {
                CardController controller = Instantiate(cardControllerPrefab, rootRect, false);
                controller.name = $"card_{i}";
                controller.transform.localPosition = new Vector3(-cardXOffset + cardSpacing * i, 125f, 0f);
                
                CardVisual cVisual = Instantiate(cardVisualPrefab, controller.transform, false);
                controller.Initialize(cardIds[i], cVisual, owner, cardIds[i], this);
                cardControllers.Add(controller);
            }
        }
        
        private void Clear()
        {
            foreach (CardController card in cardControllers)
            {
                Destroy(card.gameObject);
            }
            cardControllers.Clear();
        }

        public void SetSelectedCard(CardController cc)
        {
            draggingCard = cc;
        }

        public void UnsetSelectedCard()
        {
            draggingCard = null;
        }

        public void Update()
        {
            if (!draggingCard) return;

            float localX = draggingCard.transform.localPosition.x;

            int targetIndex = Mathf.RoundToInt((localX + (cardControllers.Count - 1) * cardSpacing / 2) / cardSpacing);
            targetIndex = Mathf.Clamp(targetIndex, 0, cardControllers.Count - 1);

            currentInsertIndex = targetIndex;
            
            UpdateCardPosition(targetIndex);
        }

        private void UpdateCardPosition(int insertIndex)
        {
            int visualIndex = 0;

            foreach (var card in cardControllers)
            {
                if (card == draggingCard) continue;

                if (visualIndex == insertIndex)
                    visualIndex++;

                Vector3 targetPos = new Vector3(
                    -((cardControllers.Count - 1) * cardSpacing) / 2 + visualIndex * cardSpacing,
                    125f,
                    0
                );

                card.transform.localPosition = Vector3.Lerp(
                    card.transform.localPosition,
                    targetPos,
                    Time.deltaTime * 10f
                );

                visualIndex++;
            }
        }

        public void PlaceDraggedCard()
        {
            if (draggingCard == null) return;

            cardControllers.Remove(draggingCard);
            cardControllers.Insert(currentInsertIndex, draggingCard);

            UpdateHierarchy();
            UpdateIndices();

            Vector3 targetPos = GetSlotPosition(currentInsertIndex);

            draggingCard.transform.DOLocalMove(targetPos, 0.25f);
            
            draggingCard = null;
        }

        private void UpdateHierarchy()
        {
            for (int i = 0; i < cardControllers.Count; i++)
            {
                cardControllers[i].transform.SetSiblingIndex(i);
            }
        }

        private void UpdateIndices()
        {
            for (int i = 0; i < cardControllers.Count; i++)
            {
                cardControllers[i].SetIndex(i);
            }
        }
        
        public Vector3 GetSlotPosition(int index)
        {
            float startX = -((cardControllers.Count - 1) * cardSpacing) / 2f;

            return new Vector3(
                startX + index * cardSpacing,
                125f,
                0f
            );
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            visual.BackgroundAlpha(0.6f, 1, 0.3f);
            visual.BorderScale(0.1f, 0.1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            visual.BackgroundAlpha(1f, 0.6f, 0.3f);
            visual.BorderScale(0f, 0.1f);
        }
    }
}
