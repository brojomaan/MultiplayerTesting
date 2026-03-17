using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Cards
{
    public class CardDockController : MonoBehaviour
    {
        [SerializeField] private CardController cardControllerPrefab;
        [SerializeField] private CardVisual cardVisualPrefab;
        [SerializeField] private RectTransform rootRect;

        [SerializeField] private List<CardController> cardControllers;

        [SerializeField] private float cardSpacing;

        public void ShowHand(int[] cardIds, NetworkPlayer owner)
        {
            Clear();

            int index = 0;
            foreach (int id in cardIds)
            {
                CardController controller = Instantiate(cardControllerPrefab, rootRect);
                CardVisual visual = Instantiate(cardVisualPrefab, controller.transform);
                controller.Initialize(id, visual, owner, id);
                cardControllers.Add(controller);
                index++;
            }

            index = 0;
            
            foreach (CardController cc in cardControllers)
            {
                float cardXOffset = (cardControllers.Count) * cardSpacing / 2;

                float targetPosition = -cardXOffset + (cardSpacing * index);

                cc.transform.localPosition = new Vector3(targetPosition, 0f, 0f);

                index++;
            }
        }
        
        private void Clear()
        {
            foreach (Transform child in this.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
